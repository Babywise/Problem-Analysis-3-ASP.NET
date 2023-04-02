using System.ComponentModel.DataAnnotations;

namespace VendorInvoicing.Request_Entities
{
    public class AddLineItemRequest
    {
        public int vendorId { get; set; }
        public int invoiceId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(0.00, double.MaxValue, ErrorMessage = "Amount must be a value zero or greater.")]
        public double? Amount { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Description must be 0-16 characters long")]
        public string? Description { get; set; }
    }
}
