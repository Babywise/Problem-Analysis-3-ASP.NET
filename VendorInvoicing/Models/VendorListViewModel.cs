using Microsoft.AspNetCore.Mvc;
using VendorInvoicing.Entities;

namespace VendorInvoicing.Models
{
    public class VendorListViewModel
    {
        public ICollection<Vendor> vendors { get; set; }
        public int? DeletedVendorId { get; set; }
        public string? startingLetter { get; set; }
        public string? endingLetter { get; set; }
    }
}
