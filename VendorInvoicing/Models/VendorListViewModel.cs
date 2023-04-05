using Microsoft.AspNetCore.Mvc;
using VendorInvoicingClassLibrary.Entities;

namespace VendorInvoicing.Models
{
    public class VendorListViewModel
    {
        public ICollection<Vendor>? vendors { get; set; }
        public int? DeletedVendorId { get; set; }
        public bool? BackgroundDeleteAllowed { get; set; } = false;
        public string? startingLetter { get; set; }
        public string? endingLetter { get; set; }
    }
}
