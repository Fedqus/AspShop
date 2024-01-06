using kurs.Attributes;
using kurs.Models;
using kurs.Models.Forms;
using kurs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace kurs.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly List<Func<List<User>, string, List<User>>> _filters;
        private readonly UserManager<User> _userManager;
        private readonly CountryManager _countryManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 CountryManager countryManager,
                                 DatabaseContext context,
                                 RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _countryManager = countryManager;
            _context = context;
            _roleManager = roleManager;
            //<option value="0">Firstname</option>
            //<option value="1">Lastname</option>
            //<option value="2">Email</option>
            //<option value="3">Country</option>
            //<option value="4">City</option>
            //<option value="5">Address</option>
            //<option value="6">Roles</option>

            _filters = new List<Func<List<User>, string, List<User>>>
            {
                (x, s) => x.Where(e => e.Firstname.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Lastname.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.UserName.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.City.Country.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.City.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Address.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => _userManager.GetRolesAsync(e).Result.Select(x => x.ToLower()).Contains(s.ToLower())).ToList(),
            };
        }
        [AjaxOnly]
        public IActionResult Table(int? field, string? search)
        {
            var entities = _userManager.Users.ToList();
            if (_filters != null && !string.IsNullOrEmpty(search))
            {
                try
                {
                    entities = _filters[field ?? 0].Invoke(entities, search);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return View(entities);
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await GetCurrentUser());
        }
        public IActionResult Register()
        {
            ViewBag.Countries = _context.Countries.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterForm model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Email already exists!");
                    return View();
                }
                var modelUser = model.User;
                var result = await _userManager.CreateAsync(modelUser, model.Password);
                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(modelUser, "Admin");
                    await _signInManager.PasswordSignInAsync(model.User.UserName, model.Password, true, false);
                    return Redirect("/account");
                }
                ModelState.AddModelError(nameof(model.Password), string.Join(", ", result.Errors.Select(x => x.Description)));
            }
            ViewBag.Countries = await _context.Countries.ToListAsync();
            return View();
        }
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginForm model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Email not found!");
                    return View();
                }
                var reuslt = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (!reuslt.Succeeded)
                {
                    ModelState.AddModelError(nameof(model.Password), "Incorrect password!");
                    return View();
                }
                return Redirect(returnUrl ?? "/");
            }
            return View();
        }
        [AjaxOnly]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await GetCurrentUser();
            var form = new ProfileForm
            {
                Firstname = currentUser.Firstname,
                Lastname = currentUser.Lastname,
                Email = currentUser.Email
            };
            return View(form);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileForm form)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUser();
                currentUser.Firstname = form.Firstname;
                currentUser.Lastname = form.Lastname;
                await _userManager.UpdateAsync(currentUser);
            }
            return View(form);
        }
        [AjaxOnly]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordForm form)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUser();
                
                var result = await _userManager.ChangePasswordAsync(currentUser, form.CurrentPassword, form.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ChangePassword));
                }
                var error = result.Errors.First();
                var key = error.Code == "PasswordMismatch" ? nameof(form.CurrentPassword) : nameof(form.NewPassword);
                ModelState.AddModelError(key, error.Description);
            }
            return View(form);
        }
        [AjaxOnly]
        public async Task<IActionResult> Address()
        {
            ViewBag.Countries = _countryManager.GetEntities();
            var currentUser = await GetCurrentUser();
            var form = new AddressForm
            {
                CityId = currentUser.CityId,
                Address = currentUser.Address
            };
            return View(form);
        }
        [HttpPost]
        public async Task<IActionResult> Address(AddressForm form)
        {
            ViewBag.Countries = _countryManager.GetEntities();
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUser();
                currentUser.CityId = form.CityId;
                currentUser.Address = form.Address;
                await _userManager.UpdateAsync(currentUser);
            }
            return View(form);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        private async Task<User> GetCurrentUser()
        {
            return await _userManager.FindByNameAsync(User.Identity?.Name);
        }
    }
}
