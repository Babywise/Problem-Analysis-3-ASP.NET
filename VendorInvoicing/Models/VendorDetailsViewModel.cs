using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.ComponentModel.DataAnnotations;
using VendorInvoicing.Components;
using VendorInvoicingClassLibrary.Entities;
using VendorInvoicingClassLibrary.Request_Entities;

namespace VendorInvoicing.Models
{
    public class VendorDetailsViewModel
    {
        public Vendor vendor { get; set; }
        public int? SelectedInvoiceId { get; set; }
        public string? startingLetter { get; set; }
        public string? endingLetter { get; set; }
        public InvoiceDetailsViewModel? InvoiceDetailsViewModel { get; set; }
        public InvoiceLineItemsViewModel? InvoiceLineItemsViewModel { get; set; }
    }
}
