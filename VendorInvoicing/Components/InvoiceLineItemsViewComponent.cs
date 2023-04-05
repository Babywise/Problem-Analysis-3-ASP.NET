using Microsoft.AspNetCore.Mvc;
using VendorInvoicingClassLibrary.Services;
using VendorInvoicingClassLibrary.Request_Entities;

namespace VendorInvoicing.Components
{
    public class InvoiceLineItemsViewComponent : ViewComponent
    {
        private IVendorInvoicingService _vendorInvoicingService;

        public InvoiceLineItemsViewComponent(IVendorInvoicingService vendorInvoicingService)
        {
            _vendorInvoicingService = vendorInvoicingService;
        }
        //InvoiceLineItemsViewModel Controller for ViewComponent (Middle man between Parent View and ViewComponent)
        public async Task<IViewComponentResult> InvokeAsync(int vendorId, int invoiceId, string startingLetter, string endingLetter) {
            InvoiceLineItemsViewModel invoiceLineItemsViewModel = new InvoiceLineItemsViewModel()
            {
                vendorId = vendorId,
                addLineItemRequest = new AddLineItemRequest(),
                invoice = _vendorInvoicingService.GetInvoiceById(invoiceId),
                startingLetter = startingLetter,
                endingLetter = endingLetter
            };
            return View(invoiceLineItemsViewModel); 
        }
    }
}
