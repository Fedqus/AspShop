namespace kurs.Models
{
    public class CartItem
    {
        public CartItem(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}
