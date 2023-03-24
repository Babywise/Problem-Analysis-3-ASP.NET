using Microsoft.AspNetCore.Mvc;
using System;
using VendorInvoicing.Entities;
using VendorInvoicing.Models;
using VendorInvoicing.Services;

namespace VendorInvoicing.Controllers
{
    public class VendorController : Controller
    {
        private VendorsContext _vendorsContext;
        private IVendorInvoicingService _vendorInvoicingService;
        public VendorController(VendorsContext vendorsContext, IVendorInvoicingService vendorInvoicingService) {
            _vendorsContext = vendorsContext;
            _vendorInvoicingService = vendorInvoicingService;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Vendor/List/")]
        public IActionResult GetVendorList([FromQuery]VendorViewModel vendorViewModel)
        {
            if (vendorViewModel.startingLetter != null && vendorViewModel.endingLetter != null)
            {
                vendorViewModel.vendors = _vendorInvoicingService.GetVendorsInRangeAZ(vendorViewModel.startingLetter, vendorViewModel.endingLetter);
            }
            else
            {
                vendorViewModel.vendors = _vendorInvoicingService.GetAllVendors();
            }
            return View("List", vendorViewModel);
        }
        [HttpPost("/Vendor/List/")]
        public IActionResult GetSortedVendorList([FromQuery]VendorViewModel vendorViewModel)
        {
            if (vendorViewModel.startingLetter != null && vendorViewModel.endingLetter != null)
            {
                if (Char.IsLetter(Convert.ToChar(vendorViewModel.startingLetter)) && Char.IsLetter(Convert.ToChar(vendorViewModel.endingLetter)))
                {
                    return RedirectToAction("GetVendorList", "Vendor", vendorViewModel);
                }
            }
            return RedirectToAction("GetVendorList", "Vendor");
        }

        public IActionResult Edit(int id)
        {
            Vendor vendor = _vendorInvoicingService.GetVendorById(id);

            if (vendor != null)
            {
                return View(vendor);
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }
        public IActionResult GetInvoicesForVendor(int id)
        {
            Vendor vendor = _vendorInvoicingService.GetVendorById(id);

            if (vendor != null)
            {
                return View("Invoices", vendor);
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }

        [HttpPost()]
        public IActionResult Delete(int id)
        {
            Vendor vendor = _vendorInvoicingService.GetVendorById(id);

            if (vendor != null)
            {
                if (_vendorInvoicingService.DeleteVendorById(id))
                {
                    TempData["LastActionMessage"] = $"Successfully deleted {vendor.Name}.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete {vendor.Name}.";
                }
                return RedirectToAction("GetVendorList", "Vendor");
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }
    }
}
