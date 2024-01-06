using kurs.Models;
using System.Text.Json;

namespace kurs.Services
{
    public class CartManager
    {
        private const string COOKIE_CART_KEY = "cart";

        private readonly IHttpContextAccessor _httpContext;
        private readonly ProductManager _productManager;
        private readonly ShippingManager _shippingManager;
        private CartModelWrapper _cart;

        public CartManager(IHttpContextAccessor httpContext, ProductManager productManager, ShippingManager shippingManager)
        {
            _httpContext = httpContext;
            _productManager = productManager;
            _shippingManager = shippingManager;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var cookieValue = _httpContext.HttpContext?.Request.Cookies[COOKIE_CART_KEY] ?? throw new ArgumentNullException();
                _cart = JsonSerializer.Deserialize<CartModelWrapper> (cookieValue) ?? throw new ArgumentNullException();
            }
            catch (Exception)
            {
                _cart = new CartModelWrapper();
                _cart.ShippingId = _shippingManager.GetEntities().First().Id;
                SaveChanges();
            }
        }
        public void SaveChanges()
        {
            if (_cart != null)
            {
                _httpContext.HttpContext?.Response.Cookies.Append(COOKIE_CART_KEY, JsonSerializer.Serialize(_cart));
            }
        }
        public CartModel GetModel()
        {
            var model = new CartModel
            {
                Items = new List<CartItem>(_cart.Items.Select(x => new CartItem(_productManager.GetEntity(x.ProductId), x.Amount))),
                Shipping = _shippingManager.GetEntity(_cart.ShippingId)
            };
            return model;
        }
        public CartItemWrapper? GetItem(int productId)
        {
            return _cart.Items.FirstOrDefault(x => x.ProductId == productId);
        }
        public void AddItem(CartItemWrapper item)
        {
            if (_cart.Items.Any(x => x.ProductId == item.ProductId))
            {
                var cartItem = GetItem(item.ProductId);
                cartItem.Amount += item.Amount;
                return;
            }
            _cart.Items.Add(item);
        }
        public void AddItem(int productId, int amount)
        {
            AddItem(new CartItemWrapper(productId, amount));
        }
        public void SetShippingId(int id)
        {
            _cart.ShippingId = id;
        }

        public void DeleteItem(int? productId)
        {
            if (productId != null)
            {
                _cart.Items.Remove(GetItem((int)productId));
            }
        }
        public void Clear()
        {
            _cart.Items.Clear();
        }
        public int ItemsCount()
        {
            return _cart.Items.Count;
        }
    }
}
