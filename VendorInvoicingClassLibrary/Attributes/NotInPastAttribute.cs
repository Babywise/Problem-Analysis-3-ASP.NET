using System.ComponentModel.DataAnnotations;

namespace VendorInvoicingClassLibrary.Attributes
{
    //Attribute class to ensure input data is in the future
    public class NotInPastAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime date;
            if (DateTime.TryParse(value?.ToString(), out date))
            {
                return date >= DateTime.Today;
            }
            return false;
        }
    }
}
