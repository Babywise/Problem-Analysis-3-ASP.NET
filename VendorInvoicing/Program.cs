using Microsoft.EntityFrameworkCore;
using VendorInvoicing.Components;
using VendorInvoicing.Entities;
using VendorInvoicing.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// get the connection string from appsettings:
string connStr = builder.Configuration.GetConnectionString("VendorsDB");
// and then use that conn'n string as we add our DB context to the
// DI container's services, specifying that we are using SQL server:
builder.Services.AddDbContext<VendorsContext>(options => options.UseSqlServer(connStr));
builder.Services.AddScoped<IVendorInvoicingService, DbVendorInvoicingService>();
builder.Services.AddScoped<InvoiceDetailsViewComponent>();
builder.Services.AddScoped<InvoiceLineItemsViewComponent>();
builder.Services.AddSingleton<IHostedService, ClearDeletedVendorsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
