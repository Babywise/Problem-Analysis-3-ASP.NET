using VendorInvoicingClassLibrary.Entities;
using VendorInvoicingClassLibrary.Request_Entities;

namespace VendorInvoicing.Components
{
    public class InvoiceDetailsViewModel
    {
        public int? vendorId { get; set; }
        public int? SelectedInvoiceId { get; set; }
        public string? startingLetter { get; set; }
        public string? endingLetter { get; set; }
        public ICollection<Invoice>? invoices { get; set; }
        public AddInvoiceRequest? addInvoiceRequest { get; set; }
        public ICollection<PaymentTerms>? PaymentTerms { get; set; }
    }
}
