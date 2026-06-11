using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Energy.Api.Common.Authorization;
using Energy.Api.Common.Middleware;
using Energy.Application.Identity.Services;
using Energy.Infrastructure;
using Energy.Infrastructure.Identity;
using Energy.Infrastructure.Identity.Services;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Seeding;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

// Disable the legacy inbound claim mapping that rewrites "sub" → ClaimTypes.NameIdentifier,
// "unique_name" → ClaimTypes.Name, etc. We rely on the raw JWT claim names (sub, sst, ...)
// inside OnTokenValidated and CurrentUser, so the mapping must be turned off
// BEFORE the JwtBearer handler is constructed.
System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------
// Single appsettings.json with per-environment sections.
// The active environment is taken from the top-level "Environment" key in
// appsettings.json (set manually, e.g. "Development" / "Production"), NOT from
// ASPNETCORE_ENVIRONMENT. We flatten the matching section onto the configuration
// root so the rest of the app keeps reading "Jwt", "ConnectionStrings",
// "Logging", ... as usual. Added last => highest precedence.
// --------------------------------------------------------------------
var selectedEnvironment = builder.Configuration["Environment"] ?? builder.Environment.EnvironmentName;
// Drive the host environment from the manual key too, so IsDevelopment()/
// IsProduction() checks (PortGuard, etc.) follow the same selection.
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

builder.Services.AddControllers().AddEnergyDataAnnotationsLocalization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEnergyLocalization();

// --------------------------------------------------------------------
// Reverse proxy / TLS termination (production: nginx -> Kestrel over HTTP)
// The proxy forwards the original scheme & client IP via X-Forwarded-* headers.
// Without this the API would see "http" + the proxy IP instead of the real
// "https" + client IP, breaking IP-based audit logs and absolute URL building.
// --------------------------------------------------------------------
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Trust the local reverse proxy. Cleared because the proxy runs on the same
    // host / private network and is not in the default loopback known-proxy list.
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("x-api-version"));
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// --------------------------------------------------------------------
// JWT bearer with security-stamp validation
// --------------------------------------------------------------------
var jwt = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
          ?? throw new InvalidOperationException("Jwt configuration is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var auth = ctx.Request.Headers.Authorization.ToString();
                var preview = string.IsNullOrEmpty(auth)
                    ? "<missing>"
                    : (auth.Length > 32 ? auth[..32] + "...(+" + (auth.Length - 32) + ")" : auth);
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");
                logger.LogInformation("[JWT recv] {Method} {Path} Authorization={Auth}",
                    ctx.Request.Method, ctx.Request.Path, preview);
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");
                logger.LogWarning(ctx.Exception,
                    "[JWT fail] {Method} {Path} -> {Type}: {Message}",
                    ctx.Request.Method, ctx.Request.Path,
                    ctx.Exception.GetType().Name, ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = async ctx =>
            {
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");
                var principal = ctx.Principal!;
                var subRaw = principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
                var stampRaw = principal.FindFirst(JwtTokenService.SecurityStampClaim)?.Value;
                if (!Guid.TryParse(subRaw, out var userId) || !Guid.TryParse(stampRaw, out var stamp))
                {
                    logger.LogWarning("[JWT validate] Invalid claims. sub={Sub} sst={Stamp}", subRaw, stampRaw);
                    ctx.Fail("Invalid token claims.");
                    return;
                }
                var db = ctx.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
                var current = await db.Users.AsNoTracking()
                    .Where(u => u.Id == userId)
                    .Select(u => new { u.SecurityStamp, u.IsActive })
                    .FirstOrDefaultAsync();
                if (current is null)
                {
                    logger.LogWarning("[JWT validate] User {UserId} not found in DB.", userId);
                    ctx.Fail("Token is no longer valid.");
                    return;
                }
                if (!current.IsActive)
                {
                    logger.LogWarning("[JWT validate] User {UserId} is inactive.", userId);
                    ctx.Fail("Token is no longer valid.");
                    return;
                }
                if (current.SecurityStamp != stamp)
                {
                    logger.LogWarning("[JWT validate] SecurityStamp MISMATCH for {UserId}. token={TokenStamp} db={DbStamp}",
                        userId, stamp, current.SecurityStamp);
                    ctx.Fail("Token is no longer valid.");
                    return;
                }
                logger.LogInformation("[JWT validate] OK for user {UserId}", userId);
            },
            OnChallenge = async ctx =>
            {
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");
                logger.LogWarning(
                    "[JWT challenge] {Method} {Path} AuthFailure={Failure} Error={Error} Desc={Desc}",
                    ctx.Request.Method, ctx.Request.Path,
                    ctx.AuthenticateFailure?.Message ?? "<none>",
                    ctx.Error ?? "<none>", ctx.ErrorDescription ?? "<none>");
                ctx.HandleResponse();
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                ctx.Response.ContentType = "application/json";
                var localizer = ctx.HttpContext.RequestServices
                    .GetRequiredService<Microsoft.Extensions.Localization.IStringLocalizer<SharedResource>>();
                await ctx.Response.WriteAsync(JsonSerializer.Serialize(
                    BaseResponse<object>.Failure(
                        localizer[LocalizationKeys.Messages.AuthenticationRequired].Value,
                        new[] { localizer[LocalizationKeys.Messages.BearerTokenInvalidOrMissing].Value })));
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document, externalResource: null)] = new List<string>()
    });
});

var app = builder.Build();

await app.RunSystemSeedingAsync();

// Must run before any middleware that inspects the request scheme / remote IP
// so they observe the original values forwarded by the reverse proxy.
app.UseForwardedHeaders();

// Swagger is intentionally enabled in ALL environments (including Production)
// so the API can be explored at /swagger regardless of ASPNETCORE_ENVIRONMENT.
app.UseSwagger();
app.UseSwaggerUI();

app.UseEnergyRequestLocalization();
// NOTE: HTTPS redirection is intentionally NOT enabled here. This is an internal
// API consumed by Energy.Web over HttpClient; a 307 to HTTPS would cause .NET's
// HttpClient to follow the redirect and strip the Authorization header, which
// then surfaces as a spurious 401 in AuthHeaderHandler. TLS termination should
// be handled by the reverse proxy / ingress in production.
app.UseRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<PermissionAuthorizationMiddleware>();
app.UseAuthorization();

app.MapControllers();

// In development, free the configured ports first: if a previous instance is
// still holding the port, kill it so we can bind instead of crashing with
// "address already in use".
if (app.Environment.IsDevelopment())
{
    Energy.Api.Common.Hosting.PortGuard.FreeConfiguredPorts(app.Configuration, app.Logger);
}

app.Run();
