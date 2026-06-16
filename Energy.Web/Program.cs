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
// Ortam bazlı bölümler içeren tek bir appsettings.json.
// Aktif ortam, appsettings.json içindeki üst düzey "Environment" anahtarından
// alınır (elle ayarlanır, ör. "Development" / "Production"); ASPNETCORE_ENVIRONMENT'tan
// DEĞİL. Eşleşen bölümü yapılandırma köküne düzleştiririz; böylece uygulamanın geri
// kalanı "Api", "Brand", "Logging", ... değerlerini her zamanki gibi okur.
// En son eklenir => en yüksek öncelik.
// --------------------------------------------------------------------
var selectedEnvironment = builder.Configuration["Environment"] ?? builder.Environment.EnvironmentName;
// Host ortamını da elle ayarlanan anahtardan yönet; böylece IsDevelopment()/
// IsProduction() kontrolleri (HSTS, HTTPS yönlendirme, güvenli çerez) aynı seçimi izler.
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
        // Sıra önemlidir: ApiExceptionFilter, PageAccessFilter'dan önce çalışmalıdır;
        // böylece eski/geçersiz bir JWT (ApiUnauthorizedException olarak yüzeye çıkan),
        // geliştirici istisna sayfasına yükselmek yerine /account/login'e temiz bir
        // yönlendirmeye dönüştürülür.
        options.Filters.AddService<ApiExceptionFilter>();
        options.Filters.AddService<PageAccessFilter>();
        // Başarısız BaseResponse zarflarını (JSON vekil eylemleri tarafından HTTP 200 ile
        // döndürülen) 400'e yükselt; böylece istemci bunları hata olarak değerlendirir.
        options.Filters.Add<EnvelopeStatusResultFilter>();
    })
    .AddJsonOptions(options =>
    {
        // Enum değerleri tel üzerinde string olarak serileştirilir (UI/JS uyumluluğu).
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    })
    .AddEnergyMvcLocalization();

builder.Services.AddEnergyLocalization();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SectionName));
builder.Services.Configure<BrandSettings>(builder.Configuration.GetSection(BrandSettings.SectionName));

// --------------------------------------------------------------------
// Ters proxy / TLS sonlandırma (üretim: nginx -> HTTP üzerinden Kestrel).
// UseHttpsRedirection, HSTS ve güvenli çerez politikasının, proxy tarafından
// X-Forwarded-Proto ile iletilen orijinal "https" şemasını görmesi için gereklidir.
// --------------------------------------------------------------------
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

// --------------------------------------------------------------------
// Veri Koruma (Data Protection).
// Kimlik doğrulama çerezi ("energy.auth") artık DataProtection anahtar halkasına
// bağlı değildir: statik bir HMAC anahtarıyla korunur (aşağıdaki "Auth:CookieProtectionKey"
// ve HmacTicketDataFormat'a bakın); böylece oturumlar, yazılabilir veya kalıcı bir
// anahtar deposu OLMADAN yeniden başlatma / app-pool geri dönüşümü / ölçek genişletme
// sonrasında korunur.
//
// DataProtection yine de kayıtlıdır çünkü antiforgery ve çerez TempData onu kullanır.
// Anahtar halkasını kalıcılaştırmak artık İSTEĞE BAĞLIDIR: antiforgery jetonlarının
// yeniden başlatmalar arasında geçerli kalmasını istiyorsanız "DataProtection:KeysPath"
// değerini yazılabilir bir klasöre ayarlayın; ayarlı değilse veya yazılamıyorsa bellek
// içi anahtarlara geri düşeriz (yalnızca o anda açık olan formlar yeniden başlatmada
// bozulur — oturumlar etkilenmez).
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
        // Yazılamayan bir yol başlatmayı çökertmemelidir; oturumlar artık buna
        // bağlı değildir. Geçici (ephemeral) anahtarlara geri dön ve devam et.
        Console.Error.WriteLine(
            $"[DataProtection] Could not persist keys to '{keysPath}': {ex.Message}. " +
            "Falling back to in-memory keys (antiforgery tokens reset on restart).");
    }
}

// Bellek içi önbellek, rol/menü gezinme aramalarını destekler; böylece her
// istekte API'ye gitmeyiz.
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// API client stack (per-user token reader + handlers + concrete clients).
builder.Services.AddEnergyApiClients();

// Web-side application services.
builder.Services.AddScoped<IAuthCookieFactory, AuthCookieFactory>();
builder.Services.AddScoped<INavigationService, NavigationService>();

// API 401/403'ünü yönlendirmelere dönüştüren istisna filtresi.
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<PageAccessFilter>();

// Çerez kimlik doğrulaması: API tarafından üretilen JWT, AuthenticationProperties.StoreTokens
// aracılığıyla bilet içinde saklanır ve giden HttpClient işleyici zinciri tarafından
// geri okunur.
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
        // Üretimde site HTTPS üzerinden sunulur (TLS proxy'de sonlandırılır, şema
        // UseForwardedHeaders ile geri yüklenir); bu yüzden güvenli çerez zorunlu
        // kılınır; geliştirmede düz HTTP üzerinden SameAsRequest'e geri dönülür.
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;

        // Kimlik doğrulama çerezini DataProtection anahtar halkası yerine yapılandırmadan
        // gelen statik bir HMAC anahtarıyla koru. Bu, yazılabilir/kalıcı bir anahtar
        // deposuna (ör. C:\Energy\keys\web) olan ihtiyacı ortadan kaldırır: anahtar,
        // yeniden başlatmalar ve örnekler arasında kararlıdır; böylece üretilen çerezler
        // geçerli kalır. Anahtar yapılandırılmamışsa varsayılan DataProtection tabanlı
        // biçimi koruruz (ör. yerel geliştirme için).
        var cookieProtectionKey = builder.Configuration["Auth:CookieProtectionKey"];
        if (!string.IsNullOrWhiteSpace(cookieProtectionKey))
        {
            options.TicketDataFormat = new HmacTicketDataFormat(cookieProtectionKey);
        }
    });

builder.Services.AddAuthorization(options =>
{
    // Varsayılan olarak her şeyi kilitle; login / access-denied eylemlerinde
    // [AllowAnonymous] ile dışında bırak.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Gerçek zamanlı sohbet taşıması (aynı köken, çerezle kimlik doğrulanır).
// Hub mesajlarının istemci tarafı alan adlarıyla eşleşmesi için camelCase yükleri
// zorunlu kıl (uygulamanın geri kalanı — MVC JSON — de camelCase'dir). Bu olmadan
// varsayılan SignalR JSON protokolü PascalCase üretir ve tarayıcı tanımsız alanları okur.
builder.Services
    .AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Hub ve tüm göndericiler tarafından paylaşılan bellek içi çevrimiçi/çevrimdışı varlık izleyici.
builder.Services.AddSingleton<Energy.Web.Hubs.IChatPresenceTracker, Energy.Web.Hubs.ChatPresenceTracker>();

var app = builder.Build();

// Önce çalışmalıdır; böylece ardışık düzenin geri kalanı (HTTPS yönlendirme, HSTS,
// güvenli çerezler) proxy tarafından iletilen orijinal şemayı / istemci IP'sini görür.
app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
// Not: HTTPS yönlendirmesi, Web→API çağrı zincirini yalnızca HTTP üzerinde tutmak ve
// proje http profili ile başlatıldığında çıkan gürültülü "Failed to determine the
// https port for redirect" uyarısını önlemek için geliştirmede bilinçli olarak devre dışıdır.

app.UseEnergyRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Her Web katmanı isteğini (sayfa + MVC/JSON aksiyonları) API üzerinden tek denetim
// havuzuna kaydet. Kaydın oturum açmış kullanıcıya atfedilmesi için kimlik doğrulamadan
// sonra yerleştirilir ve yanıtı yakalamak için uç nokta yürütmesini sarar.
app.UseMiddleware<Energy.Web.Common.Middleware.WebRequestLoggingMiddleware>();

app.MapStaticAssets().AllowAnonymous();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<Energy.Web.Hubs.ChatHub>("/hubs/chat").RequireAuthorization();

app.Run();
