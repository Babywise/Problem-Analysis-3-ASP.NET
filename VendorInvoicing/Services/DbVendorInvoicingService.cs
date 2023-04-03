using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public bool AddInvoice(Invoice invoice)
        {
            _vendorsContext.Invoices.Add(invoice);

            return _vendorsContext.SaveChanges() != 0 ? true : false;

        }

        public bool AddInvoiceLineItem(InvoiceLineItem invoiceLineItem)
        {
            _vendorsContext.InvoiceLineItems.Add(invoiceLineItem);
            return _vendorsContext.SaveChanges() != 0 ? true : false;
        }

        public bool AddVendor(Vendor vendor)
        {
            _vendorsContext.Vendors.Add(vendor);
            return _vendorsContext.SaveChanges() != 0 ? true : false;
        }

        public bool DeleteAllisDeletedVendors()
        {
            var deletedVendors = _vendorsContext.Vendors.Where(v => v.IsDeleted == true)
                .Include(v => v.Invoices)
                    .ThenInclude(i => i.InvoiceLineItems)
                .ToList();

            if (deletedVendors.Count() > 0)
            {
                foreach (var vendor in deletedVendors)
                {
                    foreach (var invoice in vendor.Invoices)
                    {
                        foreach (var invoiceLineItem in invoice.InvoiceLineItems)
                        {
                            _vendorsContext.InvoiceLineItems.Remove(invoiceLineItem);
                        }
                        _vendorsContext.Invoices.Remove(invoice);
                    }
                    _vendorsContext.Vendors.Remove(vendor);
                }
                _vendorsContext.SaveChanges();
                return true;
            }
            return false;
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

        public bool FinalDeleteVendorById(int id)
        {
            Vendor vendor = _vendorsContext.Vendors.Where(v => v.VendorId == id).FirstOrDefault();
            if (vendor != null)
            {
                _vendorsContext.Remove(vendor);
                _vendorsContext.SaveChanges();
                return true;
            }
            return false;
        }

        public ICollection<Invoice> GetAllInvoices(int vendorId)
        {
            return _vendorsContext.Invoices.Where(i => i.VendorId == vendorId).ToList();
        }

        public ICollection<Vendor> GetAllVendors()
        {
            ICollection<Vendor> vendors;
            vendors = _vendorsContext.Vendors
                .Include(v => v.Invoices)
                    .ThenInclude(i => i.InvoiceLineItems)
                .OrderBy(v => v.Name)
                .ToList();
            if (vendors != null)
            {
                foreach (Vendor v in vendors)
                {
                    foreach (Invoice invoice in v.Invoices)
                    {
                        invoice.PaymentTerms = _vendorsContext.PaymentTerms
                            .OrderBy(pt => pt.PaymentTermsId)
                            .ToList();
                    }
                }
            }
            
            return vendors;
        }

        public int? GetDeletedVendorId()
        {
            return _vendorsContext.Vendors?.Where(v => v.IsDeleted == true).FirstOrDefault()?.VendorId;
        }

        public Invoice GetInvoiceById(int id)
        {
            return _vendorsContext.Invoices.Where(i => i.InvoiceId == id).FirstOrDefault();
        }

        public Vendor GetVendorById(int id)
        {
            Vendor vendor = _vendorsContext.Vendors
                .Where(v => v.VendorId == id)
                .Include(v => v.Invoices)
                    .ThenInclude(i => i.InvoiceLineItems)
                .Include(v => v.Invoices)
                    .ThenInclude(i => i.PaymentTerm)
                .OrderBy(v => v.Name)
                .FirstOrDefault();
            if (vendor != null)
            {
                foreach (Invoice invoice in vendor.Invoices)
                {
                    invoice.PaymentTerms = _vendorsContext.PaymentTerms
                        .OrderBy(pt => pt.PaymentTermsId)
                        .ToList();
                }
            }

            return vendor;
        }

        public ICollection<Vendor> GetVendorsInRangeAZ(string startingLetter, string endingLetter)
        {
            return _vendorsContext.Vendors
                .OrderBy(v => v.Name)
                .Where(v => v.Name.CompareTo(startingLetter) >= 0 && v.Name.CompareTo(endingLetter) <= 0)
                .ToList();
        }

        public bool UndoDeleteVendorById(int id)
        {
            Vendor vendor = _vendorsContext.Vendors.Where(v => v.VendorId == id).FirstOrDefault();
            if (vendor != null)
            {
                vendor.IsDeleted = false;
                _vendorsContext.Vendors.Update(vendor);
                _vendorsContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateVendor(Vendor vendor)
        {
            Vendor vendorFromDB = _vendorsContext.Vendors.Where(v => v.VendorId == vendor.VendorId).FirstOrDefault();
            if (vendorFromDB != null)
            {
                _vendorsContext.Vendors.Update(vendor);
                _vendorsContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
