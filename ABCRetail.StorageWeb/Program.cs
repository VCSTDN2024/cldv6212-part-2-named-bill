using ABCRetail.StorageWeb.Options;
using ABCRetail.StorageWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AzureStorageOptions>()
    .Bind(builder.Configuration.GetSection(AzureStorageOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<StorageClientFactory>();
builder.Services.AddSingleton<CustomerProfileService>();
builder.Services.AddSingleton<ProductCatalogService>();
builder.Services.AddSingleton<MediaStorageService>();
builder.Services.AddSingleton<OperationsQueueService>();
builder.Services.AddSingleton<ContractsFileService>();
builder.Services.AddHostedService<StorageInitializationHostedService>();

// I'm wiring up Razor Pages so the storage demo screens actually render.
builder.Services.AddRazorPages();

var app = builder.Build();

// I tighten the HTTP pipeline to stay production ready while still supporting local dev.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // I leave HSTS at the default 30 days so browsers keep enforcing HTTPS after first contact.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
