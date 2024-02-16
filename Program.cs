using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using News.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options => 
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/AccessDenied";
})
.AddGoogle(options => 
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:clientId").Value!;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:clientSecret").Value!;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<NewsContext>(options => {
	options.UseSqlServer(builder.Configuration.GetConnectionString("connect"));
});

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
