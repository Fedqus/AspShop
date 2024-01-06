using kurs.Models;
using kurs.Models.Forms;
using kurs.Services;
using Microsoft.AspNetCore.Mvc;

namespace kurs.Controllers
{
    public class CompaniesController : EntityController<Company, CompanyForm>
    {
        public CompaniesController(CompanyManager manager) : base(manager)
        {
            _filters = new List<Func<List<Company>, string, List<Company>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList(),
            };
        }

        public override Task<IActionResult> Edit(CompanyForm form, int? id)
        {
            ModelState.Remove(nameof(form.Image));
            return base.Edit(form, id);
        }

    }
}
