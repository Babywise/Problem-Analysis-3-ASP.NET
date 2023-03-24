using Microsoft.EntityFrameworkCore;
using VendorInvoicing.Entities;
using VendorInvoicing.Models;

namespace VendorInvoicing.Services
{
    public class DbVendorInvoicingService : IVendorInvoicingService
    {
        private VendorsContext _vendorsContext;
        public DbVendorInvoicingService(VendorsContext vendorInvoicingService)
        {
            _vendorsContext = vendorInvoicingService;
        }

        public bool DeleteVendorById(int id)
        {
            Vendor vendor = _vendorsContext.Vendors.Where(v => v.VendorId == id).FirstOrDefault();
            if (vendor != null)
            {
                vendor.IsDeleted = true;
                _vendorsContext.Vendors.Update(vendor);
                _vendorsContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Vendor> GetAllVendors()
        {
            return _vendorsContext.Vendors
                .OrderBy(v => v.Name)
                .ToList();
        }

        public Vendor GetVendorById(int id)
        {
            return _vendorsContext.Vendors
                .Find(id);
        }

        public List<Vendor> GetVendorsInRangeAZ(string startingLetter, string endingLetter)
        {
            return _vendorsContext.Vendors
                .OrderBy(v => v.Name)
                .Where(e => e.Name.CompareTo(startingLetter) >= 0 && e.Name.CompareTo(endingLetter) <= 0)
                .ToList();
        }
    }
}
