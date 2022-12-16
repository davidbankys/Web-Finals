using CareerConnect.Infrastructure.DbContexts;
using CareerConnect.Infrastructure.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var configuration = builder.Configuration;
services.AddLogging();
services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Add database contexts
services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(AccountDbContext))));
services.AddDbContext<CoreDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(CoreDbContext))));

services.AddHttpContextAccessor();
services.AddScoped<AuthenticationService>();
services.AddScoped<JobService>();
services.AddScoped<UserInformation>();
services.AddScoped<ResumeService>();
services.AddScoped<UserService>();

services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Lockout.MaxFailedAccessAttempts = 10;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(60);
    }).AddEntityFrameworkStores<AccountDbContext>();

services.AddSession(options =>
{
    options.Cookie.Name = ".career.connect.session";
});

services.AddAuthentication()
    .AddCookie(options =>
    {
        options.Cookie.Name = ".career.connect.cookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/LogIn";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Job}/{action=Index}/{id?}").RequireAuthorization();

app.Run();
