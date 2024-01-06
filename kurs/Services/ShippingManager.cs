using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class ShippingManager : EntityManager<Shipping, ShippingForm>
    {
        public ShippingManager(DatabaseContext context) : base(context) { }

        public override Shipping CreateEntity(ShippingForm form)
        {
            var entity = new Shipping
            {
                Name = form.Name,
                Price = form.Price
            };
            return entity;
        }

        public override ShippingForm CreateForm(Shipping entity)
        {
            var form = new ShippingForm
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price
            };
            return form;
        }

        public override async Task Update(Shipping entity, ShippingForm form)
        {
            entity.Name = form.Name;
            entity.Price = form.Price;
            await _context.SaveChangesAsync();
        }
    }
}
