namespace kurs.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
