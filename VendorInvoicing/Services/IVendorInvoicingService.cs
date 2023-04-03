using VendorInvoicing.Entities;

namespace VendorInvoicing.Services
{
    public interface IVendorInvoicingService
    {
        public ICollection<Invoice> GetAllInvoices(int vendorId);
        public Invoice GetInvoiceById(int id);
        public bool AddInvoice(Invoice invoice);
        public bool AddInvoiceLineItem(InvoiceLineItem invoiceLineItem);
        public int? GetDeletedVendorId();
        public bool DeleteVendorById(int id);
        public bool UndoDeleteVendorById(int id);
        public bool UpdateVendor(Vendor vendor);
        public ICollection<Vendor> GetAllVendors();
        public Vendor GetVendorById(int id);
        public ICollection<Vendor> GetVendorsInRangeAZ(string startingLetter, string endingLetter);
    }
}
