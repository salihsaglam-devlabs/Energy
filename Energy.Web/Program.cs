using Energy.Localization;
using Energy.Web.Clients.Infrastructure;
using Energy.Web.Common.Filters;
using Energy.Web.Configuration;
using Energy.Web.Services.Authentication;
using Energy.Web.Services.Navigation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.AddService<PageAccessFilter>();
    })
    .AddEnergyMvcLocalization();

builder.Services.AddEnergyLocalization();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SectionName));
builder.Services.Configure<BrandSettings>(builder.Configuration.GetSection(BrandSettings.SectionName));

// In-memory cache backs the role/menu navigation lookups so we do not hit
// the API on every request.
builder.Services.AddMemoryCache();

// API client stack (per-user token reader + handlers + concrete clients).
builder.Services.AddEnergyApiClients();

// Web-side application services.
builder.Services.AddScoped<IAuthCookieFactory, AuthCookieFactory>();
builder.Services.AddScoped<IRoleIdResolver, RoleIdResolver>();
builder.Services.AddScoped<INavigationService, NavigationService>();

// Exception filter that converts API 401/403 into redirects.
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<PageAccessFilter>();

// Cookie authentication: the JWT issued by the API is stored inside the
// ticket via AuthenticationProperties.StoreTokens and read back by the
// outbound HttpClient handler chain.
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.LogoutPath = "/account/logout";
        options.AccessDeniedPath = "/account/access-denied";
        options.ReturnUrlParameter = "returnUrl";
        options.SlidingExpiration = false;
        options.Cookie.Name = "energy.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

builder.Services.AddAuthorization(options =>
{
    // Lock down everything by default; opt-out via [AllowAnonymous] on
    // login / access-denied actions.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseEnergyRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets().AllowAnonymous();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
