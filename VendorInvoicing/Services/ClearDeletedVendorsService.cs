using VendorInvoicingClassLibrary.Services;

public class ClearDeletedVendorsService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ClearDeletedVendorsService> _logger;

    public ClearDeletedVendorsService(IServiceScopeFactory serviceScopeFactory, ILogger<ClearDeletedVendorsService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    //Use Db service to find and delete any "IsDeleted" vendors every 10 seconds
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000, stoppingToken); // Wait 10 seconds before performing delete IsDeleted vendors from db
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var vendorInvoicingService = scope.ServiceProvider.GetRequiredService<IVendorInvoicingService>();
                vendorInvoicingService.DeleteAllisDeletedVendors();
            }
        }
    }
}