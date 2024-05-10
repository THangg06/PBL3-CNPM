//<<<<<<< HEAD
//using Demo.Controllers;
//using Demo.Data;
//using Demo.Helper;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<Web01Context>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("WebDemo"));
//});

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});



//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
//builder.Services.AddSession(options =>
//{
  
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session
//    options.Cookie.HttpOnly = true; // Tùy chọn bảo mật
//    options.Cookie.IsEssential = true; // Đảm bảo cookie session được sử dụng
//});
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(p =>
//    {
//        p.LoginPath = "/Login.html";
//        p.AccessDeniedPath = "/";
//    } 
//    );
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseSession();
//app.UseRouting();
//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
//=======
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Notyf;
using Demo.Controllers;
using Demo.Data;
using Demo.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Web01Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebDemo"));
});




    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddSession(options =>
{
  
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session
    options.Cookie.HttpOnly = true; // Tùy chọn bảo mật
    options.Cookie.IsEssential = true; // Đảm bảo cookie session được sử dụng
});

builder.Services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(p =>
    {
        p.LoginPath = "/Login.html";
        p.AccessDeniedPath = "/";
    } 
    );
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
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

 app.UseEndpoints(endpoints =>
{
    
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

 
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228
