namespace kurs.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }
        public int TagGroupId { get; set; }

        public virtual TagGroup TagGroup { get; set; }
    }
}
