using GasStationApp;
using GasStationApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  
        options.AccessDeniedPath = "/Account/AccessDenied"; 
        options.LogoutPath = "/Account/Logout";  
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddScoped<AddFuelController>();
builder.Services.AddScoped<SellController>();

using (var connection = new SqlConnection("Server=DESKTOP-2PSBQS1; Database=GasStationDb;TrustServerCertificate=true; User=sa; Password=1;"))
{
    try
    {
        connection.Open();
        Console.WriteLine("Bağlantı başarılı!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Bağlantı hatası: " + ex.Message);
    }
}

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
