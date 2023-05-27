#pragma warning disable CS8604
using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Repository;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Service;
using PortfolioV2.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepository, UserRepository>(x => new UserRepository(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<IInquiryRepository, InquiryRepository>(x => new InquiryRepository(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInquiryService, InquiryService>();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/User/Login";
    opts.LogoutPath = "/User/Logout";
});

WebApplication? app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "api",
    pattern: "{area:exists}/{controller}/{action}"
);

App.IsDeployed = builder.Environment.IsProduction();

app.Run();