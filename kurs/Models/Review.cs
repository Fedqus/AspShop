namespace kurs.Models
{
    public class Review
    {
        public int Id { get; set; }
        public float Rating { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }

    }
}
