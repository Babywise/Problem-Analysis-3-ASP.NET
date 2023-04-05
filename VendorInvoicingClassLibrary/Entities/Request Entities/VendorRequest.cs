using System.ComponentModel.DataAnnotations;
using VendorInvoicingClassLibrary.Attributes;

namespace VendorInvoicingClassLibrary.Request_Entities
{
    public class VendorRequest
    {
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter an address.")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "Please enter a city.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Please enter a province or state.")]
        [ProvinceOrStateInNA(ErrorMessage = "Please enter a valid province or state abbreviation.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Province or state must be a 2-letter abbreviation. (Ex. \"AB\")")]
        public string? ProvinceOrState { get; set; }
        [Required(ErrorMessage = "Please enter a zip or postal code.")]
        [RegularExpression(@"^(?:[A-Za-z]\d[A-Za-z][- ]?\d[A-Za-z]\d|\d{5}(?:[-]\d{4})?)$", ErrorMessage = "Please enter a valid zip or postal code.")]
        [StringLength(7, MinimumLength = 5)]
        [DataType(DataType.PostalCode)]
        public string? ZipOrPostalCode { get; set; }
        [Required(ErrorMessage = "Please enter a phone number.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid US/CA phone number.")]
        [DataType(DataType.PhoneNumber)]
        public string? VendorPhone { get; set; }
        [Required(ErrorMessage = "Please enter a Vendor Last Name.")]
        public string? VendorContactLastName { get; set; }
        [Required(ErrorMessage = "Please enter a Vendor First Name.")]
        public string? VendorContactFirstName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? VendorContactEmail { get; set; }
        public string? startingLetter { get; set; }
        public string? endingLetter { get; set; }

    }
}
