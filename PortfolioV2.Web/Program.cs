#pragma warning disable CS8604
using PortfolioV2.Repository.Interfaces;
using PortfolioV2.Repository;
using PortfolioV2.Service.Interfaces;
using PortfolioV2.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMySqlRepository, MySqlRepository>(x => new MySqlRepository(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInquiryRepository, InquiryRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInquiryService, InquiryService>();

builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/User/Login");

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "api",
    pattern: "{area:exists}/{controller}/{action}"
);

app.Run();