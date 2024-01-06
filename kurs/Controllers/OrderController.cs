using kurs.Attributes;
using kurs.Models;
using kurs.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace kurs.Controllers
{
    public class OrderController : Controller
    {
        private readonly List<Func<List<Order>, string, List<Order>>> _filters;
        private readonly UserManager<User> _userManager;
        private readonly OrderManager _orderManager;
        private readonly CartManager _cartManager;

        public OrderController(UserManager<User> userManager,
                               OrderManager orderManager,
                               CartManager cartManager)
        {
            _filters = new List<Func<List<Order>, string, List<Order>>>
            {
                (x, s) => x.Where(e => e.Id == int.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.User.Fullname().ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Shipping.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.TotalPrice() >= float.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.TotalPrice() <= float.Parse(s)).ToList(),
            };
            _userManager = userManager;
            _orderManager = orderManager;
            _cartManager = cartManager;
        }
        [AjaxOnly]
        public IActionResult Table(int? field, string? search)
        {
            var entities = _orderManager.GetEntities();
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

        [AjaxOnly]
        public async Task<IActionResult> Make()
        {
            var model = _cartManager.GetModel();
            if (model.Items.Count < 1)
            {
                return BadRequest("Your cart is empty!");
            }
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return BadRequest("To place an order, you must be logged in!");
            }
            foreach (var item in model.Items)
            {
                if (item.Product.Amount < item.Amount)
                {
                    return BadRequest($"{item.Product.Name} has only {item.Product.Amount} left!");
                }
            }

            var order = new Order
            {
                UserId = (await GetCurrentUser()).Id,
                ShippingId = model.Shipping.Id,
                SubtotalPrice = model.Items.Sum(x => x.Product.Price * x.Amount)
            };
            await _orderManager.Add(order);
            await _orderManager.AddProducts(order, model.Items);
            _cartManager.Clear();
            _cartManager.SaveChanges();
            return Ok();
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            return View(_orderManager.GetEntity(id));
        }
        private async Task<User> GetCurrentUser()
        {
            return await _userManager.FindByNameAsync(User.Identity?.Name);
        }
    }
}
