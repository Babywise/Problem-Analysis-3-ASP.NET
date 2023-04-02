using VendorInvoicing.Entities;
using VendorInvoicing.Request_Entities;

namespace VendorInvoicing.Components
{
    public class InvoiceLineItemsViewModel
    {
        public int? vendorId { get; set; }
        public Invoice? invoice { get; set; }
        public AddLineItemRequest? addLineItemRequest { get; set; }
    }
}
