#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Repository;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Service;
using PortfolioV2.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using PortfolioV2.Web.Custom_Code;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AWSConfig awsConfig = builder.Configuration.GetSection("AWSConfig").Get<AWSConfig>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMySqlRepository, MySqlRepository>(x => {
    return new MySqlRepository(
        builder.Configuration.GetConnectionString("Database"),
        Log.Logger
    );
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInquiryRepository, InquiryRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInquiryService, InquiryService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opts => opts.LoginPath = "/user/login"); 

builder.Services.AddRazorPages();
builder.Services.AddRouting(opts => opts.LowercaseUrls = true);

WebApplication? app = builder.Build();

LogInitializer logger = new(
    awsConfig.Id,
    awsConfig.Secret
);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
    app.UseHttpsRedirection();
    logger.ConfigureLogger("Portfolio.Web");
}
else
{    
    logger.ConfigureLogger("Portfolio.Web.Development");
}

app.UseStatusCodePagesWithRedirects("/error");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Portfolio}/{action=Home}/{id?}"
);

app.MapAreaControllerRoute(
    name: "api",
    areaName: "api",
    pattern: "api/{controller}/{action}/{id?}"
);

App.IsDeployed = builder.Environment.IsProduction();

app.Run();