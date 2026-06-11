using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Energy.Api.Common.Authorization;
using Energy.Api.Common.Middleware;
using Energy.Application;
using Energy.Infrastructure;
using Energy.Infrastructure.Identity;
using Energy.Infrastructure.Seeding;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddEnergyDataAnnotationsLocalization();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEnergyLocalization();
builder.Services.AddApiVersioning(options =>
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

// ------------------------------------------------------------------
// JWT Bearer Authentication + Permission-claim Authorization
// ------------------------------------------------------------------
var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>() ?? throw new InvalidOperationException(
        LocalizationText.Get(LocalizationKeys.Messages.JwtConfigMissing, "Jwt configuration section is missing."));

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
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        // Return standardized BaseResponse JSON for auth failures.
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
                var payload = BaseResponse<object>.Failure(
                    localizer.GetText(LocalizationKeys.Messages.AuthenticationRequired, "Authentication is required."),
                    [localizer.GetText(LocalizationKeys.Messages.BearerTokenInvalidOrMissing, "Missing or invalid bearer token.")]);
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
                var payload = BaseResponse<object>.Failure(
                    localizer.GetText(LocalizationKeys.Auth.AccessDeniedTitle, "Access denied."),
                    [localizer.GetText(LocalizationKeys.Messages.PermissionDeniedAction, "You do not have permission to perform this action.")]);
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define the JWT bearer scheme so Swagger UI shows the "Authorize" button.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = LocalizationText.Get(LocalizationKeys.Messages.SwaggerJwtDescription, "Paste your JWT access token here (no 'Bearer ' prefix needed).")
    });

    // Apply the requirement globally. The AllowAnonymousSecurityFilter below
    // strips it from anonymous operations so they don't show the lock icon.
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document, externalResource: null)] = new List<string>()
    });

    options.OperationFilter<AllowAnonymousSecurityFilter>();
});

var app = builder.Build();

// Bring the system catalog (permissions, menus, admin user, role links and
// localization overrides) up to date before the first request is served.
// Discover every [Authorize(Policy = ...)] used by controllers so the matching
// permission rows are auto-created on every startup.
var discoveredPolicyCodes = PermissionDiscovery.DiscoverPolicyCodes(typeof(Program).Assembly);
await app.RunSystemSeedingAsync(discoveredPolicyCodes);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEnergyRequestLocalization();
app.UseHttpsRedirection();
app.UseRequestLogging();
app.UseAuthentication();
app.UseMiddleware<AccessRuleEnforcementMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.Run();
