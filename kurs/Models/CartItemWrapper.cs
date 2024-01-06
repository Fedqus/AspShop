namespace kurs.Models
{
    public class CartItemWrapper
    {
        public CartItemWrapper(int productId, int amount)
        {
            ProductId = productId;
            Amount = amount;
        }

        public int ProductId { get; set; }
        public int Amount { get; set; }

    }
}
