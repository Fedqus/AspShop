namespace kurs.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
