using Microsoft.AspNetCore.Mvc;
using VendorInvoicing.Models;
using VendorInvoicing.Services;

namespace VendorInvoicing.Components
{
    public class InvoiceLineItemsViewComponent : ViewComponent
    {
        private IVendorInvoicingService _vendorInvoicingService;

        public InvoiceLineItemsViewComponent(IVendorInvoicingService vendorInvoicingService)
        {
            _vendorInvoicingService = vendorInvoicingService;
        }
        //InvoiceLineItemsViewModel Controller for ViewComponent
        public async Task<IViewComponentResult> InvokeAsync(int vendorId, int invoiceId) {
            InvoiceLineItemsViewModel invoiceLineItemsViewModel = new InvoiceLineItemsViewModel()
            {
                vendorId = vendorId,
                addLineItemRequest = new Request_Entities.AddLineItemRequest(),
                invoice = _vendorInvoicingService.GetInvoiceById(invoiceId),
            };
            return View(invoiceLineItemsViewModel); 
        }
    }
}
