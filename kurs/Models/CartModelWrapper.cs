using System.Collections;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace kurs.Models
{
    public class CartModelWrapper
    {
        public List<CartItemWrapper> Items { get; set; }
        public int ShippingId { get; set; }
        public CartModelWrapper()
        {
            Items = new List<CartItemWrapper>();
        }        
    }
}
