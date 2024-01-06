using kurs.Models;
using kurs.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace kurs.Controllers
{
    public class HomeController : Controller
    {
        private readonly CategoryManager _categoryManager;
        private readonly ProductManager _productManager;
        private readonly CompanyManager _companyManager;

        public HomeController(CategoryManager categoryManager,
                              ProductManager productManager,
                              CompanyManager companyManager)
        {
            _categoryManager = categoryManager;
            _productManager = productManager;
            _companyManager = companyManager;
        }

        public IActionResult Index()
        {
            var categories = _categoryManager.GetEntities();
            ViewBag.TrandyProducts = _productManager.GetEntities();
            ViewBag.JustArrivedProducts = _productManager.GetEntities();
            ViewBag.Companies = _companyManager.GetEntities();
            return View(categories);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHendler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["Title"] = "Not Found";
                    ViewBag.Error = new { StatusCode = statusCode, Message = "Page Not Found" };
                    break;
                default:
                    ViewData["Title"] = "Error";
                    ViewBag.Error = new { StatusCode = statusCode, Message = string.Empty };
                    break;
            }
            return View("StatusCodeError");
        }

    }
}