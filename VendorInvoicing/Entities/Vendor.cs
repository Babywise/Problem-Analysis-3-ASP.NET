namespace VendorInvoicing.Entities
{
    public class Vendor
    {
        public int VendorId { get; set; }

        public string Name { get; set; } = null!;

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; } = null!;

        public string? ProvinceOrState { get; set; } = null!;

        public string? ZipOrPostalCode { get; set; } = null!;

        public string? VendorPhone { get; set; }

        public string? VendorContactLastName { get; set; }

        public string? VendorContactFirstName { get; set; }

        public string? VendorContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Nav to all Invoices:
        public List<Invoice>? Invoices { get; set; }
    }
}
