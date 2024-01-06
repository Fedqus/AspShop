using kurs.Attributes;
using kurs.Models;
using kurs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;

namespace kurs.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductManager _productManager;
        private readonly CategoryManager _categoryManager;
        private readonly CompanyManager _companyManager;
        private readonly TagGroupManager _tagGroupManager;

        public ShopController(ProductManager productManager,
                              CategoryManager categoryManager,
                              CompanyManager companyManager,
                              TagGroupManager tagGroupManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _companyManager = companyManager;
            _tagGroupManager = tagGroupManager;
        }

        public IActionResult Index(string s, int[] ct)
        {
            ViewBag.Categories = _categoryManager.GetEntities();
            ViewBag.Companies = _companyManager.GetEntities();
            ViewBag.TagGroups = _tagGroupManager.GetEntities();

            ViewBag.Search = s;
            ViewBag.SelectedCategories = ct;
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productManager.GetEntityAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.RecommendedProducts = _productManager.GetEntities();
            return View(product);
        }
        [AjaxOnly]
        public async Task<IActionResult> Products(string? search,
                                                  int? orderBy,
                                                  int[] categories,
                                                  int[] companies,
                                                  int[] tags)
        {
            var products = await _productManager.GetEntitiesAsync();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!categories.IsNullOrEmpty())
            {
                products = products.Where(x => categories.Contains(x.CategoryId)).ToList();
            }
            if (!companies.IsNullOrEmpty())
            {
                products = products.Where(x => companies.Contains(x.CompanyId)).ToList();
            }
            if (!tags.IsNullOrEmpty())
            {
                products = products.Where(x => x.Tags.Any(t => tags.Contains(t.TagId))).ToList();
            }
            var sorts = new List<Func<List<Product>, List<Product>>>
            {
                x => x.OrderBy(e => e.Name).ToList(),
                x => x.OrderByDescending(e => e.Name).ToList(),
                x => x.OrderBy(e => e.Price).ToList(),
                x => x.OrderByDescending(e => e.Price).ToList(),
            };
            products = sorts[orderBy ?? 0].Invoke(products);

            return View(products);
        }
    }
}
