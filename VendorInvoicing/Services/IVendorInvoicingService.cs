using VendorInvoicing.Entities;

namespace VendorInvoicing.Services
{
    public interface IVendorInvoicingService
    {
        public bool DeleteVendorById(int id);
        public List<Vendor> GetAllVendors();
        public Vendor GetVendorById(int id);
        public List<Vendor> GetVendorsInRangeAZ(string startingLetter, string endingLetter);
    }
}
