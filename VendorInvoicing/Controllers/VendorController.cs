using Microsoft.AspNetCore.Mvc;
using VendorInvoicing.Entities;
using VendorInvoicing.Models;

namespace VendorInvoicing.Controllers
{
    public class VendorController : Controller
    {
        private VendorsContext _vendorsContext;
        public VendorController(VendorsContext vendorsContext) {
            _vendorsContext = vendorsContext;
        }

        [HttpGet()]
        public IActionResult Index([FromQuery]VendorViewModel vendorViewModel)
        {
            if (vendorViewModel.startingLetter != null && vendorViewModel.endingLetter != null)
            {
                vendorViewModel.vendors = _vendorsContext.Vendors
                    .OrderBy(v => v.Name)
                    .Where(e => e.Name.CompareTo(vendorViewModel.startingLetter) >= 0 && e.Name.CompareTo(vendorViewModel.endingLetter) <= 0)
                    .ToList();
                return View(vendorViewModel);
            }
            else
            {
                vendorViewModel = new VendorViewModel()
                {
                    vendors = _vendorsContext.Vendors
                    .OrderBy(v => v.Name).ToList(),
                    startingLetter = "A",
                    endingLetter = "Z"
                };
            }
            return View(vendorViewModel);
        }
        [HttpPost()]
        public IActionResult List(VendorViewModel vendorViewModel)
        {
            if (vendorViewModel.startingLetter != null && vendorViewModel.endingLetter != null)
            {
                
                return RedirectToAction("Index", vendorViewModel);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Vendor vendor = _vendorsContext.Vendors
                .OrderBy(v => v.VendorId == id).FirstOrDefault();
            return View(vendor);
        }
    }
}
