using System.ComponentModel.DataAnnotations;

namespace VendorInvoicing.Request_Entities
{
    public class VendorRequest
    {
        [Required(ErrorMessage = "Please enter a name.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter an address.")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "Please enter a city.")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Please enter a province or state.")]
        [RegularExpression(@"^(AB|BC|MB|NB|NL|NT|NS|NU|ON|PE|QC|SK|YT)$", ErrorMessage = "Please enter a valid Canadian province abbreviation.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Province Or State must be 2 letter. (Ex. \"AB\")")]
        public string? ProvinceOrState { get; set; }
        [Required(ErrorMessage = "Please enter a Zip or postal code.")]
        [RegularExpression(@"^[A-Z]\d[A-Z] \d[A-Z]\d$", ErrorMessage = "Please enter a valid Canadian postal code.")]
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

    }
}
