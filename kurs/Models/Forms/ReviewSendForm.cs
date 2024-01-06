using System.ComponentModel.DataAnnotations;

namespace kurs.Models.Forms
{
    public class ReviewSendForm
    {
        [Range(1, float.MaxValue, ErrorMessage = "The Rating field is required.")]
        public float Rating { get; set; }
        public string Content { get; set; }
        public int ProductId { get; set; }
    }
}
