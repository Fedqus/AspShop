using Microsoft.AspNetCore.Routing.Constraints;
using System.Text.RegularExpressions;

namespace kurs.Models
{
    public class Order
    {
        public int Id { get; set; }
        public float SubtotalPrice { get; set; }
        public int UserId { get; set; }
        public int ShippingId { get; set; }
        
        public virtual User User { get; set; }
        public virtual Shipping Shipping { get; set; }
        public virtual ICollection<OrderProduct> Products { get; set; }

        public float TotalPrice()
        {
            return SubtotalPrice + Shipping.Price;
        }
    }
}
