using Microsoft.AspNetCore.Mvc;
using VendorInvoicingClassLibrary.Services;
using VendorInvoicingClassLibrary.Request_Entities;

namespace VendorInvoicing.Components
{
    public class InvoiceDetailsViewComponent : ViewComponent
    {
        private IVendorInvoicingService _vendorInvoicingService;

        public InvoiceDetailsViewComponent(IVendorInvoicingService vendorInvoicingService)
        {
            _vendorInvoicingService = vendorInvoicingService;
        }
        //InvoiceDetailsViewModel Controller for ViewComponent (Middle man between Parent View and ViewComponent)
        public async Task<IViewComponentResult> InvokeAsync(int vendorId, int invoiceId, string startingLetter, string endingLetter)
        {
            InvoiceDetailsViewModel invoiceDetailsViewModel = new InvoiceDetailsViewModel()
            {
                vendorId = vendorId,
                SelectedInvoiceId = invoiceId,
                addInvoiceRequest = new AddInvoiceRequest(),
                invoices = _vendorInvoicingService.GetAllInvoices(vendorId),
                PaymentTerms = _vendorInvoicingService.GetPaymentTerms(),
                startingLetter = startingLetter,
                endingLetter = endingLetter
            };
            return View(invoiceDetailsViewModel);
        }
    }
}
