using kurs.Attributes;
using kurs.Models;
using kurs.Models.Forms;
using kurs.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace kurs.Controllers
{
    public class CategoriesController : EntityController<Category, CategoryForm>
    {
        public CategoriesController(CategoryManager manager) : base(manager)
        {
            _filters = new List<Func<List<Category>, string, List<Category>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.ProductsCount >= int.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.ProductsCount <= int.Parse(s)).ToList()
            };
        }

        public override Task<IActionResult> Edit(CategoryForm form, int? id)
        {
            ModelState.Remove(nameof(form.Image));
            return base.Edit(form, id);
        }

    }
}
