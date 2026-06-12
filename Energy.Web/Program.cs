using Energy.Localization;
using Energy.Web.Clients.Infrastructure;
using Energy.Web.Common.Filters;
using Energy.Web.Configuration;
using Energy.Web.Services.Authentication;
using Energy.Web.Services.Navigation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------
// Single appsettings.json with per-environment sections.
// The active environment is taken from the top-level "Environment" key in
// appsettings.json (set manually, e.g. "Development" / "Production"), NOT from
// ASPNETCORE_ENVIRONMENT. We flatten the matching section onto the configuration
// root so the rest of the app keeps reading "Api", "Brand", "Logging", ...
// as usual. Added last => highest precedence.
// --------------------------------------------------------------------
var selectedEnvironment = builder.Configuration["Environment"] ?? builder.Environment.EnvironmentName;
// Drive the host environment from the manual key too, so IsDevelopment()/
// IsProduction() checks (HSTS, HTTPS redirect, secure cookie) follow the same selection.
builder.Environment.EnvironmentName = selectedEnvironment;
var environmentSection = builder.Configuration.GetSection(selectedEnvironment);
if (environmentSection.Exists())
{
    var environmentValues = environmentSection
        .AsEnumerable(makePathsRelative: true)
        .Where(kvp => kvp.Value is not null)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    builder.Configuration.AddInMemoryCollection(environmentValues);
}

builder.Services
    .AddControllersWithViews(options =>
    {
        // Order matters: ApiExceptionFilter must run before PageAccessFilter
        // so a stale/invalid JWT (surfaced as ApiUnauthorizedException) is
        // converted into a clean redirect to /account/login instead of
        // bubbling up to the developer exception page.
        options.Filters.AddService<ApiExceptionFilter>();
        options.Filters.AddService<PageAccessFilter>();
        // Promote failed BaseResponse envelopes (returned with HTTP 200 by the
        // JSON proxy actions) to 400 so the client treats them as errors.
        options.Filters.Add<EnvelopeStatusResultFilter>();
    })
    .AddEnergyMvcLocalization();

builder.Services.AddEnergyLocalization();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SectionName));
builder.Services.Configure<BrandSettings>(builder.Configuration.GetSection(BrandSettings.SectionName));

// --------------------------------------------------------------------
// Reverse proxy / TLS termination (production: nginx -> Kestrel over HTTP).
// Required so UseHttpsRedirection, HSTS and the secure-cookie policy observe
// the original "https" scheme forwarded by the proxy via X-Forwarded-Proto.
// --------------------------------------------------------------------
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

// --------------------------------------------------------------------
// Data Protection.
// The auth cookie ("energy.auth") no longer depends on the DataProtection key
// ring: it is protected with a static HMAC key (see "Auth:CookieProtectionKey"
// and HmacTicketDataFormat below), so sessions survive restarts / app-pool
// recycles / scale-out WITHOUT any writable or persisted key store.
//
// DataProtection is still registered because antiforgery and cookie TempData
// use it. Persisting the key ring is now OPTIONAL: set "DataProtection:KeysPath"
// to a writable folder if you want antiforgery tokens to stay valid across
// restarts; if it is unset or not writable we fall back to in-memory keys
// (only currently-open forms break on a restart — sessions are unaffected).
var keysPath = builder.Configuration["DataProtection:KeysPath"];
var dataProtection = builder.Services.AddDataProtection()
    .SetApplicationName("Energy.Web");
if (!string.IsNullOrWhiteSpace(keysPath))
{
    try
    {
        Directory.CreateDirectory(keysPath);
        dataProtection.PersistKeysToFileSystem(new DirectoryInfo(keysPath));
    }
    catch (Exception ex)
    {
        // A non-writable path must not crash startup; sessions do not rely on
        // this anymore. Fall back to ephemeral keys and carry on.
        Console.Error.WriteLine(
            $"[DataProtection] Could not persist keys to '{keysPath}': {ex.Message}. " +
            "Falling back to in-memory keys (antiforgery tokens reset on restart).");
    }
}

// In-memory cache backs the role/menu navigation lookups so we do not hit
// the API on every request.
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// API client stack (per-user token reader + handlers + concrete clients).
builder.Services.AddEnergyApiClients();

// Web-side application services.
builder.Services.AddScoped<IAuthCookieFactory, AuthCookieFactory>();
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
        // In production the site is served over HTTPS (TLS terminated at the
        // proxy, scheme restored by UseForwardedHeaders) so force a secure
        // cookie; in development over plain HTTP fall back to SameAsRequest.
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;

        // Protect the auth cookie with a static HMAC key from configuration
        // instead of the DataProtection key ring. This removes the need for a
        // writable/persisted key store (e.g. C:\Energy\keys\web): the key is
        // stable across restarts and instances, so issued cookies stay valid.
        // If the key is not configured we keep the default DataProtection-based
        // format (e.g. for local development).
        var cookieProtectionKey = builder.Configuration["Auth:CookieProtectionKey"];
        if (!string.IsNullOrWhiteSpace(cookieProtectionKey))
        {
            options.TicketDataFormat = new HmacTicketDataFormat(cookieProtectionKey);
        }
    });

builder.Services.AddAuthorization(options =>
{
    // Lock down everything by default; opt-out via [AllowAnonymous] on
    // login / access-denied actions.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Real-time chat transport (same-origin, cookie-authenticated).
// Force camelCase payloads so hub messages match the client-side field names
// (the rest of the app — MVC JSON — is camelCase too). Without this the default
// SignalR JSON protocol emits PascalCase and the browser reads undefined fields.
builder.Services
    .AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// In-memory online/offline presence tracker shared by the hub and any pushers.
builder.Services.AddSingleton<Energy.Web.Hubs.IChatPresenceTracker, Energy.Web.Hubs.ChatPresenceTracker>();

var app = builder.Build();

// Must run first so the rest of the pipeline (HTTPS redirect, HSTS, secure
// cookies) sees the original scheme / client IP forwarded by the proxy.
app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
// Note: HTTPS redirection is intentionally disabled in development to keep
// the Web→API call chain HTTP-only and avoid the noisy
// "Failed to determine the https port for redirect" warning when the
// project is launched on the http profile.

app.UseEnergyRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Audit every Web-tier request (page + MVC/JSON actions) into the single audit
// sink via the API. Placed after authentication so the entry is attributed to
// the signed-in user, and wraps endpoint execution to capture the response.
app.UseMiddleware<Energy.Web.Common.Middleware.WebRequestLoggingMiddleware>();

app.MapStaticAssets().AllowAnonymous();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<Energy.Web.Hubs.ChatHub>("/hubs/chat").RequireAuthorization();

app.Run();
