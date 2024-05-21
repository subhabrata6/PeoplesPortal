using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TestEmployeeApp.Authentication;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/").AllowAnonymousToPage("/Account/Login");
});
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//        .AddCookie(options =>
//        {
//            options.Cookie.HttpOnly = true;
//            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//            options.Cookie.SameSite = SameSiteMode.Strict;
//            options.LoginPath = "/Account/Login";
//            options.AccessDeniedPath = "/Account/Login";

//            // Configure other options as needed
//        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseDeveloperExceptionPage();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
});

//app.UseMvc(routes =>
//{
//    routes.MapRoute(
//        name: "default",
//        template: "{controller=Home}/{action=Index}/{id?}");
//});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");
});
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
});


//app.MapControllers();
//app.MapGet("/", context =>
//{
//    context.Response.Redirect("/Account/Login");
//    return Task.CompletedTask;
//});

app.Run();
