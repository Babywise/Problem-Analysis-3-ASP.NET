using System.ComponentModel.DataAnnotations;

namespace VendorInvoicing.Request_Entities
{
    public class AddInvoiceRequest
    {
        public int vendorId { get; set; }
        [Required(ErrorMessage = "Please select an invoice date")]
        [DataType(DataType.Date)]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a term option")]
        public int? PaymentTermsId { get; set; }

    }
}
