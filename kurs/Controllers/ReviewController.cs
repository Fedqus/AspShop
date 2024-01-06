using kurs.Attributes;
using kurs.Models;
using kurs.Models.Forms;
using kurs.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace kurs.Controllers
{
    public class ReviewController : EntityController<Review, ReviewSendForm>
    {
        private readonly UserManager<User> _userManager;
        private readonly ProductManager _productManager;

        public ReviewController(ReviewManager manager, UserManager<User> userManager, ProductManager productManager) : base(manager)
        {
            _filters = new List<Func<List<Review>, string, List<Review>>>
            {
                (x, s) => x.Where(e => e.User.Fullname().ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Product.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Content.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Rating >= float.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.Rating <= float.Parse(s)).ToList()
            };
            _userManager = userManager;
            _productManager = productManager;
        }
        
        [AjaxOnly]
        public async Task<IActionResult> ProductReviews(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }
            var product = await _productManager.GetEntityAsync(productId);
            return View(product);
        }
        [AjaxOnly]
        public IActionResult Send(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }
            ReviewSendForm form = new ReviewSendForm
            {
                ProductId = (int)productId
            };
            return View(form);
        }
        [HttpPost]
        public async Task<IActionResult> Send(ReviewSendForm form)
        {
            if (ModelState.IsValid)
            {
                var entity = _manager.CreateEntity(form);
                entity.UserId = (await GetCurrentUser()).Id;
                var result = await _manager.AddAsync(entity);
            }
            return View(form);
        }
        private async Task<User> GetCurrentUser()
        {
            return await _userManager.FindByNameAsync(User.Identity?.Name);
        }

        public override IActionResult Add()
        {
            return NotFound();
        }
        public override Task<IActionResult> Edit(int? id)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }
    }
}
