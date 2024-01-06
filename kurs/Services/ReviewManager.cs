using kurs.Migrations;
using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class ReviewManager : EntityManager<Review, ReviewSendForm>
    {
        public ReviewManager(DatabaseContext context) : base(context) { }

        public override Review CreateEntity(ReviewSendForm form)
        {
            var entity = new Review
            {
                Rating = form.Rating,
                Content = form.Content,
                ProductId = form.ProductId,
            };
            return entity;
        }

        public override ReviewSendForm CreateForm(Review entity)
        {
            var form = new ReviewSendForm
            {
                Rating = entity.Rating,
                Content = entity.Content,
                ProductId = entity.ProductId
            };
            return form;
        }

        public override async Task Update(Review entity, ReviewSendForm form)
        {
            entity.Rating = form.Rating;
            entity.Content = form.Content;
            await _context.SaveChangesAsync();
        }
    }
}
