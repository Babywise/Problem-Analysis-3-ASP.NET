using Microsoft.AspNetCore.Mvc;
using VendorInvoicing.Entities;

namespace RESTfulStateTransfer.Controllers
{
    public class VendorController : Controller
    {
        private VendorsContext _vendorsContext;
        public VendorController(VendorsContext vendorsContext) {
            _vendorsContext = vendorsContext;
        }

        public IActionResult Index()
        {
            List<Vendor> vendors = _vendorsContext.Vendors
                .OrderBy(v => v.Name).ToList();
            return View(vendors);
        }

        public IActionResult Edit(int id)
        {
            Vendor vendor = _vendorsContext.Vendors
                .OrderBy(v => v.VendorId == id).FirstOrDefault();
            return View(vendor);
        }
    }
}
