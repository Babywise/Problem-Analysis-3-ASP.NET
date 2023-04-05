using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Numerics;
using VendorInvoicing.Components;
using VendorInvoicing.Entities;
using VendorInvoicing.Entities.Request_Entities;
using VendorInvoicing.Models;
using VendorInvoicing.Request_Entities;
using VendorInvoicing.Services;

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

        [HttpGet()]
        public IActionResult Index()
        {
            return RedirectToAction("GetVendorList", "Vendor");
        }

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
        [HttpGet("/Vendor/{vendorId}/Invoices/")]
        public IActionResult GetInvoicesForVendor(int vendorId, [FromQuery] string startingLetter, [FromQuery] string endingLetter)
        {
            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel()
            {
                vendor = _vendorInvoicingService.GetVendorById(vendorId),
                startingLetter = startingLetter,
                endingLetter = endingLetter
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

        [HttpGet("/Vendor/{vendorId}/Invoices/{invoiceId}")]
        public IActionResult GetInvoicesForVendor(int vendorId, int invoiceId)
        {
            VendorDetailsViewModel vendorDetailsViewModel = new VendorDetailsViewModel()
            {
                vendor = _vendorInvoicingService.GetVendorById(vendorId),
            };
            if (vendorDetailsViewModel.vendor != null && vendorDetailsViewModel.vendor.Invoices.Where(i => i.InvoiceId == invoiceId).FirstOrDefault() != null)
            {
                vendorDetailsViewModel.InvoiceLineItemsViewModel = new InvoiceLineItemsViewModel()
                {
                    addLineItemRequest = new AddLineItemRequest(),
                    invoice = _vendorInvoicingService.GetInvoiceById(invoiceId),
                    vendorId = vendorId,
                };
                vendorDetailsViewModel.SelectedInvoiceId = invoiceId;
                return View("Invoices", vendorDetailsViewModel);
            }
            else
            {
                TempData["ErrorMessage"] = "Error finding selected entry.";
                return RedirectToAction("GetVendorList", "Vendor");
            }
        }

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
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = invoiceLineItemsViewModel.addLineItemRequest.vendorId, invoiceId = invoiceLineItem.InvoiceId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding Invoice";
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = invoiceLineItemsViewModel.addLineItemRequest.vendorId, invoiceId = invoiceId });
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
            };
            return View("Invoices", vendorDetailsViewModel);
        }

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
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = vendorId, invoiceId = invoiceDetailsViewModel.SelectedInvoiceId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding Invoice";
                    return RedirectToAction("GetInvoicesForVendor", "Vendor", new { vendorId = invoiceDetailsViewModel.addInvoiceRequest.vendorId });
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
            };
            return View("Invoices", vendorDetailsViewModel);
        }

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
