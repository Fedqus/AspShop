using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class CountryManager : EntityManager<Country, CountryForm>
    {
        public CountryManager(DatabaseContext context) : base(context) { }

        public override Country CreateEntity(CountryForm form)
        {
            var entity = new Country
            {
                Name = form.Name
            };
            return entity;
        }

        public override CountryForm CreateForm(Country entity)
        {
            var form = new CountryForm
            {
                Id = entity.Id,
                Name = entity.Name
            };
            return form;
        }

        public override async Task Update(Country entity, CountryForm form)
        {
            entity.Name = form.Name;
            await _context.SaveChangesAsync();
        }
    }
}
