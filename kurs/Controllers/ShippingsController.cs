using kurs.Attributes;
using Microsoft.AspNetCore.Mvc;
using kurs.Services;
using kurs.Models.Forms;
using kurs.Models;

namespace kurs.Controllers
{
    public class ShippingsController : EntityController<Shipping, ShippingForm>
    {
        public ShippingsController(ShippingManager manager) : base(manager)
        {
            _filters = new List<Func<List<Shipping>, string, List<Shipping>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Price >= float.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.Price <= float.Parse(s)).ToList()
            };
        }
    }
}
