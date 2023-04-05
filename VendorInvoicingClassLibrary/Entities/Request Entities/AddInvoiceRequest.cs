using System.ComponentModel.DataAnnotations;
using VendorInvoicingClassLibrary.Attributes;

namespace VendorInvoicingClassLibrary.Request_Entities
{
    public class AddInvoiceRequest
    {
        public int vendorId { get; set; }
        [Required(ErrorMessage = "Please select an invoice date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [NotInPast(ErrorMessage = "Invoice date cannot be in the past")]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a term option")]
        public int? PaymentTermsId { get; set; }

    }
}
