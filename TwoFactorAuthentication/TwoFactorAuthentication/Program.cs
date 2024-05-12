using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwoFactorAuthentication.Authentication;
using TwoFactorAuthentication.Core;

namespace TwoFactorAuthentication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthenticationServices()
                        .AddCoreServices(builder.Configuration)
                        .AddControllersWithViews();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
   .AddCookie(options =>
   {
       options.Cookie.Name = "App"; // Customize cookie name if needed
       options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set expiration time for the cookie
       options.LoginPath = "/User/Login"; // Customize login path
       //options.LogoutPath = "/Account/Logout"; // Customize logout path
       options.AccessDeniedPath = "/User/AccessDenied";
   });

        //builder.Services.AddAuthorization(options =>
        //{
        //    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        //        .RequireAuthenticatedUser()
        //        .Build();
        //});
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AccessDenied", policy =>
                policy.RequireAuthenticatedUser());
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //// Clear cookies programmatically
        //app.Use(async (context, next) =>
        //{
        //    context.Response.Cookies.Delete("App"); // Replace "App" with the name of your cookie
        //    await next.Invoke();
        //});



        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseSession();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=Login}/{id?}");

        app.Run();
    }
}
