using Microsoft.AspNetCore.StaticFiles;
using yevgeller_v3.Models.BdpqTestingFramework;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBdpqTestingRepository,  BdpqTestingRepository>();
builder.Services.AddSingleton<IBdpqTestingFramework, BdpqTestingFramework>();
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
provider.Mappings[".webmanifest"] = "application/manifest+json";

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions() { ContentTypeProvider = provider});
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
