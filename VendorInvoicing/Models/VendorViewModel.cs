using Microsoft.AspNetCore.Mvc;
using VendorInvoicing.Entities;

namespace VendorInvoicing.Models
{
    public class VendorViewModel
    {
        public List<Vendor> vendors { get; set; }
        [BindProperty(Name = "startingLetter")]
        public string? startingLetter { get; set; }
        [BindProperty(Name = "endingLetter")]
        public string? endingLetter { get; set; }
    }
}
