using VendorInvoicing.Services;

public class ClearDeletedVendorsService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ClearDeletedVendorsService> _logger;

    public ClearDeletedVendorsService(IServiceScopeFactory serviceScopeFactory, ILogger<ClearDeletedVendorsService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000, stoppingToken); // Wait 10 seconds
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var vendorInvoicingService = scope.ServiceProvider.GetRequiredService<IVendorInvoicingService>();
                var vendorToDelete = vendorInvoicingService.GetDeletedVendorId();

                if (vendorToDelete != null && vendorToDelete != 0)
                {
                    vendorInvoicingService.FinalDeleteVendorById((int)vendorToDelete);
                    _logger.LogInformation($"Deleted vendor with ID {vendorToDelete}");
                }
            }
        }
    }
}