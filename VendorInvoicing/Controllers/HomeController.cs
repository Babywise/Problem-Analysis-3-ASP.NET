using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VendorInvoicing.Models;

namespace VendorInvoicing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //On Home Index, redirect to the vendor index
        public IActionResult Index()
        {
            return RedirectToAction("GetVendorList", "Vendor");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}