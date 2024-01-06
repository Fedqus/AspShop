using kurs.Services;

namespace kurs.Models
{
    public class CartModel
    {
        public List<CartItem> Items { get; set; }
        public Shipping Shipping { get; set; }
    }
}
