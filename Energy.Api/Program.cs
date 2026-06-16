using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Energy.Api.Common.Authorization;
using Energy.Api.Common.Middleware;
using Energy.Application;
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

// "sub" → ClaimTypes.NameIdentifier, "unique_name" → ClaimTypes.Name vb. yeniden
// yazan eski gelen claim eşlemesini devre dışı bırak. OnTokenValidated ve CurrentUser
// içinde ham JWT claim adlarına (sub, sst, ...) güveniyoruz; bu nedenle eşleme,
// JwtBearer işleyicisi oluşturulmadan ÖNCE kapatılmalıdır.
System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------
// Ortam bazlı bölümler içeren tek bir appsettings.json.
// Aktif ortam, appsettings.json içindeki üst düzey "Environment" anahtarından
// alınır (elle ayarlanır, ör. "Development" / "Production"); ASPNETCORE_ENVIRONMENT'tan
// DEĞİL. Eşleşen bölümü yapılandırma köküne düzleştiririz; böylece uygulamanın geri
// kalanı "Jwt", "ConnectionStrings", "Logging", ... değerlerini her zamanki gibi okur.
// En son eklenir => en yüksek öncelik.
// --------------------------------------------------------------------
var selectedEnvironment = builder.Configuration["Environment"] ?? builder.Environment.EnvironmentName;
// Host ortamını da elle ayarlanan anahtardan yönet; böylece IsDevelopment()/
// IsProduction() kontrolleri (PortGuard vb.) aynı seçimi izler.
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

builder.Services.AddControllers(options =>
{
    options.Filters.Add<Energy.Api.Common.Filters.FluentValidationActionFilter>();
}).AddEnergyDataAnnotationsLocalization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// Model bağlama / DataAnnotations doğrulama hatalarını da standart BaseResponse
// zarfında döndür (spec §21: tüm API'de tek tip hata standardı). [ApiController]
// varsayılan ProblemDetails yanıtını bununla değiştiririz.
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(kvp => kvp.Value is not null && kvp.Value.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value!.Errors.Select(e =>
                string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage))
            .ToArray();

        var body = Energy.Shared.Models.V1.Common.Responses.BaseResponse<object>
            .Failure("Validation failed.", errors);
        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(body);
    };
});

builder.Services.AddEnergyApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEnergyLocalization();

// --------------------------------------------------------------------
// Ters proxy / TLS sonlandırma (üretim: nginx -> HTTP üzerinden Kestrel)
// Proxy, orijinal şemayı ve istemci IP'sini X-Forwarded-* başlıklarıyla iletir.
// Bu olmadan API, gerçek "https" + istemci IP yerine "http" + proxy IP görür ve
// IP tabanlı denetim günlükleri ile mutlak URL oluşturma bozulur.
// --------------------------------------------------------------------
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Yerel ters proxy'ye güven. Proxy aynı host / özel ağda çalıştığı ve varsayılan
    // loopback bilinen-proxy listesinde olmadığı için liste temizlenir.
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
// Güvenlik damgası doğrulamalı JWT bearer
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
    // Birden fazla controller aynı isimde iç içe tip (ör. NoteBody) tanımlayabildiğinden,
    // şema kimliğini yalnızca tip adı yerine benzersiz tam adla üretiriz; aksi halde
    // Swashbuckle "aynı schemaId zaten kullanılıyor" hatasıyla başarısız olur.
    options.CustomSchemaIds(type => (type.FullName ?? type.Name).Replace("+", ".").Replace("`", "_"));

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

// İstek şemasını / uzak IP'yi inceleyen herhangi bir ara katmandan ÖNCE çalışmalıdır;
// böylece ters proxy tarafından iletilen orijinal değerleri görürler.
app.UseForwardedHeaders();

// Swagger, ASPNETCORE_ENVIRONMENT'tan bağımsız olarak API'nin /swagger üzerinden
// keşfedilebilmesi için bilinçli olarak TÜM ortamlarda (Üretim dahil) etkindir.
app.UseSwagger();
app.UseSwaggerUI();

app.UseEnergyRequestLocalization();
// NOT: HTTPS yönlendirmesi burada bilinçli olarak etkinleştirilmemiştir. Bu, Energy.Web
// tarafından HttpClient üzerinden tüketilen dahili bir API'dir; HTTPS'e 307 yönlendirmesi,
// .NET HttpClient'ın yönlendirmeyi izleyip Authorization başlığını düşürmesine neden olur
// ve bu da AuthHeaderHandler'da sahte bir 401 olarak görünür. TLS sonlandırması, üretimde
// ters proxy / ingress tarafından yapılmalıdır.
app.UseRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<PermissionAuthorizationMiddleware>();
app.UseAuthorization();

app.MapControllers();

// Geliştirmede, yapılandırılan portları önce serbest bırak: önceki bir örnek portu
// hâlâ tutuyorsa, "address already in use" hatasıyla çökmek yerine bağlanabilmemiz
// için o süreci sonlandır.
if (app.Environment.IsDevelopment())
{
    Energy.Api.Common.Hosting.PortGuard.FreeConfiguredPorts(app.Configuration, app.Logger);
}

app.Run();
