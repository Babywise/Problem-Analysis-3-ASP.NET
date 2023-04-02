using Microsoft.AspNetCore.Mvc;
using VendorInvoicing.Services;

namespace VendorInvoicing.Components
{
    public class InvoiceDetailsViewComponent : ViewComponent
    {
        private IVendorInvoicingService _vendorInvoicingService;

        public InvoiceDetailsViewComponent(IVendorInvoicingService vendorInvoicingService)
        {
            _vendorInvoicingService = vendorInvoicingService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int vendorId, int invoiceId)
        {
            InvoiceDetailsViewModel invoiceDetailsViewModel = new InvoiceDetailsViewModel()
            {
                vendorId = vendorId,
                SelectedInvoiceId = invoiceId,
                addInvoiceRequest = new Request_Entities.AddInvoiceRequest(),
                invoices = _vendorInvoicingService.GetAllInvoices(vendorId)
            };
            return View(invoiceDetailsViewModel);
        }
    }
}
