using VendorInvoicing.Entities;
using VendorInvoicing.Request_Entities;

namespace VendorInvoicing.Components
{
    public class InvoiceDetailsViewModel
    {
        public int? vendorId { get; set; }
        public int? SelectedInvoiceId { get; set; }
        public ICollection<Invoice>? invoices { get; set; }
        public AddInvoiceRequest? addInvoiceRequest { get; set; }
    }
}
