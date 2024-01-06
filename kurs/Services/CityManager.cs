using kurs.Models;
using kurs.Models.Forms;

namespace kurs.Services
{
    public class CityManager : EntityManager<City, CityForm>
    {
        public CityManager(DatabaseContext context) : base(context) { }

        public override City CreateEntity(CityForm form)
        {
            var entity = new City
            {
                Name = form.Name,
                CountryId = form.CountryId
            };
            return entity;
        }

        public override CityForm CreateForm(City entity)
        {
            var form = new CityForm
            {
                Id = entity.Id,
                Name = entity.Name,
                CountryId = entity.CountryId
            };
            return form;
        }

        public override async Task Update(City entity, CityForm form)
        {
            entity.Name = form.Name;
            entity.CountryId = form.CountryId;
            await _context.SaveChangesAsync();
        }
    }
}
