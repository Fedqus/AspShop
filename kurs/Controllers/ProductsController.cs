using kurs.Models;
using kurs.Models.Forms;
using kurs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace kurs.Controllers
{
    public class ProductsController : EntityController<Product, ProductForm>
    {
        private readonly TagGroupManager _tagGroupManager;
        private readonly CompanyManager _companyManager;
        private readonly CategoryManager _categoryManager;

        public ProductsController(ProductManager manager,
                                  TagGroupManager tagGroupManager,
                                  CompanyManager companyManager,
                                  CategoryManager categoryManager) : base(manager)
        {
            _filters = new List<Func<List<Product>, string, List<Product>>>
            {
                (x, s) => x.Where(e => e.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Description.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Category.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Company.Name.ToLower().Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Tags.Select(t => t.Tag.Name.ToLower()).Contains(s.ToLower())).ToList(),
                (x, s) => x.Where(e => e.Price >= float.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.Price <= float.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.Amount >= int.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.Amount <= int.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.Rating >= float.Parse(s)).ToList(),
                (x, s) => x.Where(e => e.Rating <= float.Parse(s)).ToList(),
            };
            _tagGroupManager = tagGroupManager;
            _companyManager = companyManager;
            _categoryManager = categoryManager;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.TagGroups = _tagGroupManager.GetEntities();
            ViewBag.Companies = _companyManager.GetEntities();
            ViewBag.Categories = _categoryManager.GetEntities();
        }
        public override Task<IActionResult> Edit(ProductForm form, int? id)
        {
            ModelState.Remove(nameof(form.Images));
            return base.Edit(form, id);
        }

    }
}
