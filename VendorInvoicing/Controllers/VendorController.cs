using Microsoft.AspNetCore.Mvc;
using VendorInvoicing.Components;
using VendorInvoicing.DataAccess;
using VendorInvoicing.Models;
using VendorInvoicingClassLibrary.Entities;
using VendorInvoicingClassLibrary.Request_Entities;
using VendorInvoicingClassLibrary.Services;

namespace VendorInvoicing.Controllers
{
    public class VendorController : Controller
    {
        private VendorsContext _vendorsContext;
        private IVendorInvoicingService _vendorInvoicingService;
        public VendorController(VendorsContext vendorsContext, IVendorInvoicingService vendorInvoicingService)
        {
            _vendorsContext = vendorsContext;
            _vendorInvoicingService = vendorInvoicingService;
        }

        //Default Route - Redirects to Main Vendor List Page
        [HttpGet()]
        public IActionResult Index()
        {
            return RedirectToAction("GetVendorList", "Vendor");
        }

        //Retrieves Vendor List and returns it to the view to be displayed
        //Allows a range between A and Z otherwise defaults to show all
        [HttpGet("/Vendor/List/")]
        public IActionResult GetVendorList([FromQuery]VendorListViewModel vendorListViewModel)
        {
            if (vendorListViewModel.startingLetter != null && vendorListViewModel.endingLetter != null)
            {
                vendorListViewModel.vendors = _vendorInvoicingService.GetVendorsInRangeAZ(vendorListViewModel.startingLetter, vendorListViewModel.endingLetter);
            }
            else
            {
                vendorListViewModel.vendors = _vendorInvoicingService.GetAllVendors();
            }
            vendorListViewModel.DeletedVendorId = _vendorInvoicingService.GetDeletedVendorId();
            return View("List", vendorListViewModel);
        }
        
        //Get Edit Page for Vendor, Matches vendor to vendor in database using ID
        //to be displayed for editing
        [HttpGet("/Vendor/Edit/{id}")]
        public IActionResult GetEditVendor(int id, [FromQuery] string startingLetter, [FromQuery] string endingLetter)
        {
            Vendor vendor = _vendorInvoicingService.GetVendorById(id);

            if (vendor != null)
            {
                VendorRequest vendorRequest = new VendorRequest()
                {
                    Address1 = vendor.Address1,
                    Address2 = vendor.Address2,
                    City = vendor.City,
                    Name = vendor.Name,
                    ProvinceOrState = vendor.ProvinceOrState,
                    VendorContactEmail = vendor.VendorContactEmail,
                    VendorContactFirstName = vendor.VendorContactFirstName,
                    VendorContactLastName = vendor.VendorContactLastName,
                    VendorId = vendor.VendorId,
                    VendorPhone = vendor.VendorPhone,
                    ZipOrPostalCode = vendor.ZipOrPostalCode,
                    startingLetter = startingLetter,
                    endingLetter = endingLetter
                };
                return View("Edit", vendorRequest);
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }
        //Post Edit Page for Vendor, Matches vendor to vendor in database using ID
        //If found, edits are saved otherwise the current viewstate is returned back
        //to the user (Validation found in Attributes folder & VendorRequest Entity)
        [HttpPost("/Vendor/Edit/{id}")]
        public IActionResult UpdateVendor(int id, VendorRequest vendorFromView)
        {
            Vendor vendorFromDB = _vendorInvoicingService.GetVendorById(id);
            if (ModelState.IsValid)
            {
                if (vendorFromDB != null)
                {
                    vendorFromDB.Name = vendorFromView.Name;
                    vendorFromDB.Address1 = vendorFromView.Address1;
                    vendorFromDB.Address2 = vendorFromView.Address2;
                    vendorFromDB.City = vendorFromView.City;
                    vendorFromDB.ProvinceOrState = vendorFromView.ProvinceOrState;
                    vendorFromDB.ZipOrPostalCode = vendorFromView.ZipOrPostalCode;
                    vendorFromDB.VendorContactLastName = vendorFromView.VendorContactLastName;
                    vendorFromDB.VendorContactFirstName = vendorFromView.VendorContactFirstName;
                    vendorFromDB.VendorContactEmail = vendorFromView.VendorContactEmail;
                    vendorFromDB.VendorPhone = vendorFromView.VendorPhone;
                    _vendorInvoicingService.UpdateVendor(vendorFromDB);

                    TempData["LastActionMessage"] = $"Successfully modified \"{vendorFromView.Name}\".";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error finding selected entry.";
                }
                return RedirectToAction("GetVendorList", "Vendor", new { startingLetter = vendorFromView.startingLetter, endingLetter = vendorFromView.endingLetter });
            }
            return View("Edit", vendorFromView);
        }
        //Get Add Page for Vendor
        [HttpGet("/Vendor/Add/")]
        public IActionResult GetAddVendor([FromQuery] string startingLetter, [FromQuery] string endingLetter)
        {
            VendorRequest vendorRequest = new VendorRequest()
            {
                startingLetter = startingLetter,
                endingLetter = endingLetter,
            };
            return View("Add", vendorRequest);
        }
        //Post Add Page for Vendor, If valid, add to database otherwise the current
        //viewstate is returned back to the user
        //(Validation found in Attributes folder & VendorRequest Entity)
        [HttpPost("/Vendor/Add/")]
        public IActionResult AddVendor(VendorRequest vendorFromView)
        {
            if (ModelState.IsValid)
            {

                Vendor vendor = new Vendor()
                {
                    Name = vendorFromView.Name,
                    Address1 = vendorFromView.Address1,
                    Address2 = vendorFromView.Address2,
                    City = vendorFromView.City,
                    ProvinceOrState = vendorFromView.ProvinceOrState,
                    ZipOrPostalCode = vendorFromView.ZipOrPostalCode,
                    VendorContactLastName = vendorFromView.VendorContactLastName,
                    VendorContactFirstName = vendorFromView.VendorContactFirstName,
                    VendorContactEmail = vendorFromView.VendorContactEmail,
                    VendorPhone = vendorFromView.VendorPhone
                };

                if (_vendorInvoicingService.AddVendor(vendor))
                {
                    TempData["LastActionMessage"] = $"Successfully created \"{vendorFromView.Name}\".";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding vendor.";
                }
                return RedirectToAction("GetVendorList", "Vendor", new { startingLetter = vendorFromView.startingLetter, endingLetter = vendorFromView.endingLetter });
            }
            return View("Add", vendorFromView);
        }
        //Get Invoices Page for a specific Vendor, includes all sub data
        //such as invoice line items and paymentterms
        [HttpGet("/Vendor/{vendorId}/Invoices/")]
        public IActionResult GetInvoicesForVendor(int vendorId, [FromQuery] string startingLetter, [FromQuery] string endingLetter)
        {
            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel()
            {
                vendor = _vendorInvoicingService.GetVendorById(vendorId),
                startingLetter = (startingLetter != null) ? startingLetter : null,
                endingLetter = (endingLetter != null) ? endingLetter : null
            };

            if (vendorDetailsViewModel.vendor != null)
            {
                vendorDetailsViewModel.SelectedInvoiceId = vendorDetailsViewModel?.vendor?.Invoices?.FirstOrDefault()?.InvoiceId;
                vendorDetailsViewModel.InvoiceLineItemsViewModel = new InvoiceLineItemsViewModel()
                {
                    addLineItemRequest = new AddLineItemRequest(),
                    invoice = vendorDetailsViewModel?.vendor?.Invoices?.FirstOrDefault(),
                    vendorId = vendorId,
                };
                return View("Invoices", vendorDetailsViewModel);
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }
        //Get Invoices Page for a specific Vendor, includes all sub data
        //such as invoice line items and paymentterms
        [HttpGet("/Vendor/{vendorId}/Invoices/{invoiceId}")]
        public IActionResult GetInvoicesForVendor(int vendorId, int invoiceId, [FromQuery] string startingLetter, [FromQuery] string endingLetter)
        {
            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel()
            {
                vendor = _vendorInvoicingService.GetVendorById(vendorId),
            };
            if (vendorDetailsViewModel.vendor != null)
            {
                vendorDetailsViewModel.InvoiceLineItemsViewModel = new InvoiceLineItemsViewModel()
                {
                    addLineItemRequest = new AddLineItemRequest(),
                    invoice = _vendorInvoicingService.GetInvoiceById(invoiceId),
                    vendorId = vendorId,
                };
                vendorDetailsViewModel.SelectedInvoiceId = invoiceId;
                vendorDetailsViewModel.startingLetter = startingLetter;
                vendorDetailsViewModel.endingLetter = endingLetter;
                return View("Invoices", vendorDetailsViewModel);
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }
        //Post add InvoiceLineItem Controller for a specific Vendor invoice
        [HttpPost("/Vendor/{vendorId}/Invoices/{invoiceId}/AddLineItem")]
        public IActionResult AddLineItem(int vendorId, int invoiceId, InvoiceLineItemsViewModel invoiceLineItemsViewModel)
        {
            if (invoiceLineItemsViewModel.addLineItemRequest.Description != null &&
                invoiceLineItemsViewModel.addLineItemRequest.Amount != null)
            {
                InvoiceLineItem invoiceLineItem = new InvoiceLineItem()
                {
                    Amount = invoiceLineItemsViewModel.addLineItemRequest.Amount,
                    Description = invoiceLineItemsViewModel.addLineItemRequest.Description,
                    InvoiceId = invoiceLineItemsViewModel.addLineItemRequest.invoiceId,
                };
                if (_vendorInvoicingService.AddInvoiceLineItem(invoiceLineItem))
                {
                    TempData["LastActionMessage"] = $"Line Item sucessfully created for Invoice {invoiceLineItem.InvoiceId}";
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = invoiceLineItemsViewModel.addLineItemRequest.vendorId, invoiceId = invoiceLineItem.InvoiceId,
                        startingLetter = invoiceLineItemsViewModel.startingLetter, endingLetter = invoiceLineItemsViewModel.endingLetter
                    });
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding Invoice";
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = invoiceLineItemsViewModel.addLineItemRequest.vendorId, invoiceId = invoiceId,
                        startingLetter = invoiceLineItemsViewModel.startingLetter, endingLetter = invoiceLineItemsViewModel.endingLetter
                    });
                }
            }
            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel()
            {
                vendor = _vendorInvoicingService.GetVendorById(vendorId),
                InvoiceLineItemsViewModel = invoiceLineItemsViewModel,
                SelectedInvoiceId = invoiceId,
                InvoiceDetailsViewModel = new InvoiceDetailsViewModel()
                {
                    vendorId = vendorId,
                    SelectedInvoiceId = invoiceId,
                    invoices = _vendorInvoicingService.GetAllInvoices(vendorId),
                    addInvoiceRequest = new AddInvoiceRequest() 
                    {
                        vendorId = vendorId,
                    },
                },
                startingLetter = invoiceLineItemsViewModel?.startingLetter,
                endingLetter = invoiceLineItemsViewModel?.endingLetter,
            };
            return View("Invoices", vendorDetailsViewModel);
        }
        //Post add Invoice Controller for a specific Vendor
        [HttpPost("/Vendor/{vendorId}/Invoices/{invoiceId}/AddInvoice")]
        public IActionResult AddInvoice(int vendorId, InvoiceDetailsViewModel invoiceDetailsViewModel)
        {
            if (invoiceDetailsViewModel.addInvoiceRequest.InvoiceDate != null &&
                invoiceDetailsViewModel.addInvoiceRequest.PaymentTermsId != null &&
                invoiceDetailsViewModel.SelectedInvoiceId != null)
            {
                Invoice invoice = new Invoice()
                {
                    VendorId = invoiceDetailsViewModel.addInvoiceRequest.vendorId,
                    InvoiceDate = invoiceDetailsViewModel.addInvoiceRequest.InvoiceDate,
                    PaymentTermsId = (int)invoiceDetailsViewModel.addInvoiceRequest.PaymentTermsId,
                };
                if (_vendorInvoicingService.AddInvoice(invoice))
                {
                    TempData["LastActionMessage"] = $"Invoice sucessfully created for {invoice.InvoiceDate}";
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = vendorId, 
                        startingLetter = invoiceDetailsViewModel.startingLetter, endingLetter = invoiceDetailsViewModel.endingLetter
                    });
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding Invoice";
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = invoiceDetailsViewModel.addInvoiceRequest.vendorId,
                        startingLetter = invoiceDetailsViewModel.startingLetter, endingLetter = invoiceDetailsViewModel.endingLetter
                    });
                }
            }
            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel()
            {
                vendor = _vendorInvoicingService.GetVendorById(vendorId),
                InvoiceDetailsViewModel = invoiceDetailsViewModel,
                SelectedInvoiceId = invoiceDetailsViewModel.SelectedInvoiceId,
                InvoiceLineItemsViewModel = new InvoiceLineItemsViewModel()
                {
                    vendorId = vendorId,
                    invoice = _vendorInvoicingService.GetInvoiceById((int)invoiceDetailsViewModel.SelectedInvoiceId),
                    addLineItemRequest = new AddLineItemRequest()
                    {
                        vendorId = vendorId,
                        invoiceId = (int)invoiceDetailsViewModel.SelectedInvoiceId
                    }
                },
                startingLetter = invoiceDetailsViewModel?.startingLetter,
                endingLetter = invoiceDetailsViewModel?.endingLetter,
            };
            return View("Invoices", vendorDetailsViewModel);
        }
        //Post delete vendor by Id
        [HttpPost()]
        public IActionResult Delete(int id)
        {
            Vendor vendor = _vendorInvoicingService.GetVendorById(id);

            if (vendor != null)
            {
                if (_vendorInvoicingService.DeleteVendorById(id))
                {
                    TempData["LastActionMessageUndo"] = $"Successfully deleted \"{vendor.Name}\".";
                    return RedirectToAction("GetVendorList", "Vendor");
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete {vendor.Name}.";
                    return RedirectToAction("GetVendorList", "Vendor");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }
        //Post undo delete vendor by Id
        [HttpPost()]
        public IActionResult UndoDelete(int id)
        {
            Vendor vendor = _vendorInvoicingService.GetVendorById(id);

            if (vendor != null)
            {
                if (_vendorInvoicingService.UndoDeleteVendorById(id))
                {
                    TempData["LastActionMessage"] = $"\"{vendor.Name}\" Restored Successfully .";
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
