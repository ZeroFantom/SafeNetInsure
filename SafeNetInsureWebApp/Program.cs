using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.FluentValidation;
using Blazorise.Icons.FontAwesome;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SafeNetInsureWebApp;
using SafeNetInsureWebApp.Context;
using SafeNetInsureWebApp.Data.Authorization;
using SafeNetInsureWebApp.Data.Services;
using AMW = SafeNetInsureWebApp.Data.Authorization.AuthorizationMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Name = ".SageNetInsureWebApp.Session";
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

builder.Services.AddScoped<ServiceAdministrationData>();
builder.Services.AddScoped<ServiceDataUser>();
builder.Services.AddScoped<ServiceDataPolicy>();

builder.Services.AddDbContext<InsureDbContext>(
    d => d.UseMySql("Name=ConnectDb",
        ServerVersion.AutoDetect("server=localhost;port=33060;uid=root;password=Zero2004;database=bd_insurance"),
        option => option.UseNewtonsoftJson().UseNetTopologySuite()));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons()
    .AddBootstrap5Components()
    .AddBlazoriseFluentValidation();

builder.Services.AddValidatorsFromAssembly(typeof(App).Assembly);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/user_service/login";
        options.LogoutPath = "/user_service/logout";
        options.AccessDeniedPath = "/error?code=403";
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    });

builder.Services.AddSingleton<IAuthorizationHandler, OrganizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.Requirements.Add(new OrganizationRequirement(new[] { "Администратор" })));
});

builder.Services.AddCors();

var app = builder.Build();

var db = builder.Services.BuildServiceProvider().GetService<InsureDbContext>();

await db.Database.MigrateAsync();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession()
    .UseDefaultFiles()
    .UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseMiddleware<AMW>(app.Services);

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();