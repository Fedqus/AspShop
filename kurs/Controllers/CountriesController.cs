using kurs.Attributes;
using Microsoft.AspNetCore.Mvc;
using kurs.Services;
using kurs.Models.Forms;
using kurs.Models;

namespace kurs.Controllers
{
    public class CountriesController : EntityController<Country, CountryForm>
    {
        public CountriesController(CountryManager manager) : base(manager) 
        {
            _filters = new List<Func<List<Country>, string, List<Country>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList()
            };
        }
    }
}
