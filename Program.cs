using Microsoft.EntityFrameworkCore;
using ThietLap;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllersWithViews().AddNewtonsoftJson();
services.AddDbContext<Web.Contexts.WEBDbContext>(config => config.UseSqlite(configuration.GetConnectionString("Sqlite")));
services.ThemDichVuXacThuc();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

app.Run();
