using VendorInvoicingClassLibrary.Entities;
using VendorInvoicingClassLibrary.Request_Entities;

namespace VendorInvoicing.Components
{
    public class InvoiceLineItemsViewModel
    {
        public int? vendorId { get; set; }
        public string? startingLetter { get; set; }
        public string? endingLetter { get; set; }
        public Invoice? invoice { get; set; }
        public AddLineItemRequest? addLineItemRequest { get; set; }
    }
}
