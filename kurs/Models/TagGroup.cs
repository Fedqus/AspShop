namespace kurs.Models
{
    public class TagGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
