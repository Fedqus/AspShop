using kurs.Attributes;
using Microsoft.AspNetCore.Mvc;
using kurs.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using kurs.Models.Forms;
using kurs.Models;

namespace kurs.Controllers
{
    public class CitiesController : EntityController<City, CityForm>
    {
        private readonly CountryManager _countryManager;

        public CitiesController(CityManager cityManager, CountryManager countryManager) : base(cityManager)
        {
            _filters = new List<Func<List<City>, string, List<City>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Country.Name.ToLower().Contains(s.ToLower())).ToList()
            };
            _countryManager = countryManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.Countries = _countryManager.GetEntities();
        }
    }
}
