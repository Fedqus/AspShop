using kurs.Attributes;
using kurs.Models;
using kurs.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace kurs.Controllers
{
    public class CartController : Controller
    {
        private readonly CartManager _cartManager;
        private readonly ShippingManager _shippingManager;
        private readonly ProductManager _productManager;

        public CartController(CartManager cartManager,
                              ShippingManager shippingManager,
                              ProductManager productManager)
        {
            _cartManager = cartManager;
            _shippingManager = shippingManager;
            _productManager = productManager;
        }

        public IActionResult Index()
        {
            var model = _cartManager.GetModel();
            ViewBag.Shippings = _shippingManager.GetEntities();
            return View(model);
        }
        public IActionResult ThankYou()
        {
            return View();
        }
        [AjaxOnly]
        public IActionResult Table()
        {
            var model = _cartManager.GetModel();
            return View(model.Items);
        }
        [AjaxOnly]
        public IActionResult Summary()
        {
            var model = _cartManager.GetModel();
            var subtotal = model.Items.Sum(x => x.Product.Price * x.Amount);
            var shippingPrice = model.Shipping.Price;
            var tuple = Tuple.Create(subtotal, shippingPrice);
            ViewBag.Muted = _cartManager.ItemsCount() < 1;
            return View(tuple);
        }
        [AjaxOnly]
        public IActionResult Add(int? productId, int? amount)
        {
            var product = _productManager.GetEntity(productId);
            if (product == null || amount == null)
            {
                return BadRequest("Error!");
            }
            var productCartAmount = _cartManager.GetItem((int)productId)?.Amount ?? 0;
            var productAmount = product.Amount - productCartAmount;
            if (productAmount <= 0)
            {
                return BadRequest($"Out of stock!");
            }
            if (productAmount < amount)
            {
                return BadRequest($"Only {productAmount} left!");
            }

            _cartManager.AddItem((int)productId, (int)amount);
            _cartManager.SaveChanges();
            return Ok();
        }
        [AjaxOnly]
        public IActionResult Update(int? productId, int? amount)
        {
            if (productId == null || amount == null)
            {
                return BadRequest();
            }
            _cartManager.GetItem((int)productId).Amount = (int)amount;
            _cartManager.SaveChanges();
            return Ok();
        }
        [AjaxOnly]
        public IActionResult UpdateShipping(int? shippingId)
        {
            if (shippingId == null)
            {
                return BadRequest();
            }
            _cartManager.SetShippingId((int)shippingId);
            _cartManager.SaveChanges();
            return Ok();
        }
        [AjaxOnly]
        public IActionResult Delete(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }
            _cartManager.DeleteItem(productId);
            _cartManager.SaveChanges();
            return Ok();
        }
        [AjaxOnly]
        public JsonResult ItemsCount()
        {
            return Json(_cartManager.ItemsCount());
        }
    }
}
