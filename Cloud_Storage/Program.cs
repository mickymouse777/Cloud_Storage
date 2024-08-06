using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Cloud_Storage.Data;
using Cloud_Storage.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Cloud_StorageContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cloud_StorageContext") ?? throw new InvalidOperationException("Connection string 'Cloud_StorageContext' not found.")));

// Access the configuration object
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
// Register BlobService with configuration
builder.Services.AddSingleton(new BlobService(configuration.GetConnectionString("AzureStorage")));

// Register TableStorageService with configuration
builder.Services.AddSingleton(new TableStorageService(configuration.GetConnectionString("AzureStorage")));


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
