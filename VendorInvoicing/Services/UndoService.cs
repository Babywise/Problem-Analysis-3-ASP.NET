using Microsoft.EntityFrameworkCore;
using VendorInvoicing.Entities;

namespace VendorInvoicing.Services
{
    public class UndoService
    {
        private readonly VendorsContext _vendorsContext;
        private bool _BackgroundDeleteAllowed;
        public UndoService(VendorsContext vendorsContext)
        {
            _vendorsContext = vendorsContext;
            _BackgroundDeleteAllowed = false;
        }

        public void setBackgroundDeleteAllowed(bool state)
        {
            _BackgroundDeleteAllowed = state;
        }

        public bool isBackgroundDeleteAllowed()
        {
            return _BackgroundDeleteAllowed;
        }

    }
}
