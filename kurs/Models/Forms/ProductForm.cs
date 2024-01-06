namespace kurs.Models.Forms
{
    public class ProductForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public List<int> TagIds { get; set; }
        public IFormFileCollection Images { get; set; }
    }
}
