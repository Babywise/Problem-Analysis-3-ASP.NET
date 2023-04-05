using System.ComponentModel.DataAnnotations;

namespace VendorInvoicingClassLibrary.Attributes
{
    //Attribute class to ensure input data is a valid CA / US Province
    public class ProvinceOrStateInNAAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string provinceOrState = value as string;

            if (string.IsNullOrEmpty(provinceOrState))
            {
                // Allow null or empty values
                return ValidationResult.Success;
            }

            string[] validAbbreviations = new string[]
            {
                "AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT", // Canada
                "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", // US
                "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", 
                "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", 
                "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY"
            };

            if (!validAbbreviations.Contains(provinceOrState.ToUpper()))
            {
                return new ValidationResult("Please enter a valid province or state abbreviation.");
            }

            return ValidationResult.Success;
        }
    }
}