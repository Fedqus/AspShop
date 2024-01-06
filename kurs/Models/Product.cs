namespace kurs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
        public float Rating { get; set; }
        public int ReviewsCount { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual ICollection<ProductTag> Tags { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
