using System.Diagnostics;
using System.Text.Json;

// ---------------------------------------------------------------------------
// Energy.Migrator
// ---------------------------------------------------------------------------
// Etkileşimli EF Core migration yöneticisi (Energy.Publish'in tamamlayıcısı).
//
// Başlangıçta, Energy.Api/appsettings.json içinden AKTİF veritabanı sağlayıcısını
// ("Environment" + eşleşen "Database:Provider") tespit eder ve her `dotnet ef`
// komutunu o sağlayıcının kendine ait migrations projesine yönlendirir:
//     PostgreSql  -> Energy.Migrations.PostgreSql
//     SqlServer   -> Energy.Migrations.SqlServer
//
// Ardından etkileşimli bir menü gösterir ve ne yapmak istediğinizi sürekli sorar:
//   1) Migration ekle      (otomatik tarih-saat adı: M<yyyyMMddHHmm>, opsiyonel ek)
//   2) Veritabanını güncelle (en sona veya listeden seçtiğiniz bir migration'a)
//   3) Migration'ları listele
//   4) Son migration'ı kaldır
//   5) SQL script üret
//   6) Sağlayıcı değiştir (yalnızca bu oturum için geçersiz kıl)
//   0) Çıkış
//
// Geçersiz kılmalar (ortam değişkenleri):
//   ENERGY_DB_PROVIDER=SqlServer    appsettings'ten bağımsız bir sağlayıcıyı zorla
//   DOTNET=/path/to/dotnet          belirli bir dotnet çalıştırılabilirini kullan
// ---------------------------------------------------------------------------

const string contextName = "Energy.Infrastructure.Persistence.AppDbContext";

// Depo kökü: bu projenin derleme çıktısı klasörüne göre ../ 
// (Energy.Migrator/bin/Debug/net10.0 -> dört seviye yukarı = depo kökü).
var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
var appSettingsPath = Path.Combine(repoRoot, "Energy.Api", "appsettings.json");
var startupProject = Path.Combine(repoRoot, "Energy.Api", "Energy.Api.csproj");
var dotnet = Environment.GetEnvironmentVariable("DOTNET") ?? "dotnet";

// Aktif sağlayıcıyı çöz (ortam değişkeni appsettings'e göre önceliklidir).
var provider = ResolveProvider(appSettingsPath);

Console.WriteLine("==================================================");
Console.WriteLine("           Energy EF Core Migration Manager       ");
Console.WriteLine("==================================================");
Console.WriteLine($"Repo kökü     : {repoRoot}");
Console.WriteLine($"Startup proje : Energy.Api");
Console.WriteLine($"Context       : {contextName}");
PrintProvider(provider);
Console.WriteLine();

// Verify `dotnet ef` is available up-front for a clearer error message.
if (!await EnsureEfToolAsync(dotnet))
{
    Console.Error.WriteLine(
        "\n'dotnet ef' bulunamadı. Kurmak için:\n  dotnet tool install --global dotnet-ef\n");
    return 1;
}

// Interactive loop.
while (true)
{
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine($"Aktif sağlayıcı: {provider.Label}  ->  {provider.MigrationsProject}");
    Console.WriteLine("Ne yapmak istersiniz?");
    Console.WriteLine("  1) Migration ekle        (otomatik tarih-saat-dakika isimli)");
    Console.WriteLine("  2) Veritabanını güncelle (en sona veya seçtiğiniz migration'a)");
    Console.WriteLine("  3) Migration'ları listele");
    Console.WriteLine("  4) Son migration'ı kaldır");
    Console.WriteLine("  5) SQL script üret");
    Console.WriteLine("  6) Sağlayıcı değiştir (bu oturum için)");
    Console.WriteLine("  0) Çıkış");
    Console.Write("Seçim: ");

    var choice = Console.ReadLine()?.Trim();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            await AddMigrationAsync(provider);
            break;
        case "2":
            await UpdateDatabaseAsync(provider);
            break;
        case "3":
            await ListMigrationsAsync(provider);
            break;
        case "4":
            await RemoveMigrationAsync(provider);
            break;
        case "5":
            await GenerateScriptAsync(provider);
            break;
        case "6":
            provider = SwitchProvider();
            break;
        case "0":
        case "q":
        case "exit":
            Console.WriteLine("Görüşürüz!");
            return 0;
        default:
            Console.WriteLine("Geçersiz seçim, tekrar deneyin.\n");
            break;
    }
}

// ---------------------------------------------------------------------------
// Menu actions
// ---------------------------------------------------------------------------

async Task AddMigrationAsync(ProviderInfo p)
{
    // Sistematik tarih-saat adı: M<yyyyMMddHHmm>. Üretilen migration SINIF adının
    // geçerli bir C# tanımlayıcısı olması için bir harfle ön ek alır (rakamla
    // başlayamaz). İsteğe bağlı olarak okunabilir bir son ek eklenir.
    var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");
    var defaultName = $"M{timestamp}";

    Console.Write($"İsim eki (opsiyonel, boş geçilebilir) [{defaultName}]: ");
    var suffix = Console.ReadLine()?.Trim();

    var name = string.IsNullOrWhiteSpace(suffix)
        ? defaultName
        : $"{defaultName}_{Sanitize(suffix)}";

    Console.WriteLine($"\n==> [{p.Label}] migration ekleniyor: {name}");
    var exit = await RunEfAsync(p,
        "migrations", "add", name,
        "--output-dir", "Migrations");

    Console.WriteLine(exit == 0
        ? $"\n✔ Migration eklendi: {name}\n"
        : $"\n✖ Migration eklenemedi (çıkış kodu {exit}).\n");
}

async Task UpdateDatabaseAsync(ProviderInfo p)
{
    Console.WriteLine("Hedef migration'a güncellemek isterseniz adını girin.");
    Console.WriteLine("Boş bırakırsanız EN SON migration'a güncellenir.");
    Console.WriteLine("(Mevcut migration'ları görmek için önce 3 numarayı kullanın.)");
    Console.Write("Hedef migration (boş = en son): ");
    var target = Console.ReadLine()?.Trim();

    Console.Write(string.IsNullOrWhiteSpace(target)
        ? "Veritabanı EN SON migration'a güncellenecek. Onaylıyor musunuz? (e/h): "
        : $"Veritabanı '{target}' migration'ına güncellenecek. Onaylıyor musunuz? (e/h): ");
    if (!Confirm()) { Console.WriteLine("İptal edildi.\n"); return; }

    Console.WriteLine($"\n==> [{p.Label}] veritabanı güncelleniyor...");
    var exit = string.IsNullOrWhiteSpace(target)
        ? await RunEfAsync(p, "database", "update")
        : await RunEfAsync(p, "database", "update", target);

    Console.WriteLine(exit == 0
        ? "\n✔ Veritabanı güncellendi.\n"
        : $"\n✖ Güncelleme başarısız (çıkış kodu {exit}).\n");
}

async Task ListMigrationsAsync(ProviderInfo p)
{
    Console.WriteLine($"==> [{p.Label}] migration listesi\n");
    await RunEfAsync(p, "migrations", "list");
    Console.WriteLine();
}

async Task RemoveMigrationAsync(ProviderInfo p)
{
    Console.Write("Son (uygulanmamış) migration kaldırılacak. Onaylıyor musunuz? (e/h): ");
    if (!Confirm()) { Console.WriteLine("İptal edildi.\n"); return; }

    Console.WriteLine($"\n==> [{p.Label}] son migration kaldırılıyor...");
    var exit = await RunEfAsync(p, "migrations", "remove");

    Console.WriteLine(exit == 0
        ? "\n✔ Son migration kaldırıldı.\n"
        : $"\n✖ Kaldırma başarısız (çıkış kodu {exit}). Migration veritabanına uygulanmış olabilir.\n");
}

async Task GenerateScriptAsync(ProviderInfo p)
{
    Console.Write("Çıktı dosyası yolu (boş = konsola yaz): ");
    var output = Console.ReadLine()?.Trim();

    var args = new List<string> { "migrations", "script", "--idempotent" };
    if (!string.IsNullOrWhiteSpace(output))
    {
        args.Add("--output");
        args.Add(output);
    }

    Console.WriteLine($"\n==> [{p.Label}] SQL script üretiliyor...");
    var exit = await RunEfAsync(p, args.ToArray());

    Console.WriteLine(exit == 0
        ? string.IsNullOrWhiteSpace(output) ? "\n✔ Script üretildi.\n" : $"\n✔ Script yazıldı: {output}\n"
        : $"\n✖ Script üretilemedi (çıkış kodu {exit}).\n");
}

ProviderInfo SwitchProvider()
{
    Console.WriteLine("Sağlayıcı seçin:");
    Console.WriteLine("  1) PostgreSQL");
    Console.WriteLine("  2) SQL Server");
    Console.Write("Seçim: ");
    var sel = Console.ReadLine()?.Trim();
    var next = sel switch
    {
        "2" => ProviderInfo.SqlServer,
        "1" => ProviderInfo.PostgreSql,
        _ => null
    };

    if (next is null)
    {
        Console.WriteLine("Geçersiz seçim, sağlayıcı değiştirilmedi.\n");
        // Re-resolve current to return a non-null value.
        return ResolveProvider(appSettingsPath);
    }

    Console.WriteLine();
    PrintProvider(next);
    Console.WriteLine();
    return next;
}

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

// `dotnet ef <args...>` komutunu sağlayıcının migrations projesine ve paylaşılan
// başlangıç projesine karşı çalıştırır, çıktıyı canlı akıtır. Sürecin çıkış kodunu döndürür.
async Task<int> RunEfAsync(ProviderInfo p, params string[] efArgs)
{
    var projectPath = Path.Combine(repoRoot, p.MigrationsProject, $"{p.MigrationsProject}.csproj");

    var psi = new ProcessStartInfo
    {
        FileName = dotnet,
        UseShellExecute = false,
        WorkingDirectory = repoRoot,
    };
    psi.ArgumentList.Add("ef");
    foreach (var a in efArgs) psi.ArgumentList.Add(a);
    psi.ArgumentList.Add("--project");
    psi.ArgumentList.Add(projectPath);
    psi.ArgumentList.Add("--startup-project");
    psi.ArgumentList.Add(startupProject);
    psi.ArgumentList.Add("--context");
    psi.ArgumentList.Add(contextName);

    // Başlangıç uygulamasını seçtiğimiz sağlayıcıya zorla; böylece bağlantı dizesi
    // ve DbContext seçenekleri migrations projesiyle eşleşir.
    psi.Environment["ENERGY_DB_PROVIDER"] = p.ConfigValue;

    try
    {
        using var process = Process.Start(psi)
            ?? throw new InvalidOperationException("İşlem başlatılamadı.");
        await process.WaitForExitAsync();
        return process.ExitCode;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"  ! Komut çalıştırılamadı: {ex.Message}");
        return 1;
    }
}

// `dotnet ef` aracının kurulu olduğunu doğrular (aksi hâlde erken ve açık bir hata verir).
async Task<bool> EnsureEfToolAsync(string dotnetExe)
{
    try
    {
        var psi = new ProcessStartInfo
        {
            FileName = dotnetExe,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        psi.ArgumentList.Add("ef");
        psi.ArgumentList.Add("--version");

        using var process = Process.Start(psi);
        if (process is null) return false;
        await process.WaitForExitAsync();
        return process.ExitCode == 0;
    }
    catch
    {
        return false;
    }
}

bool Confirm()
{
    var answer = Console.ReadLine()?.Trim().ToLowerInvariant();
    return answer is "e" or "evet" or "y" or "yes";
}

// İsteğe bağlı migration adı son eki için yalnızca tanımlayıcı-güvenli karakterleri tutar.
static string Sanitize(string input)
{
    var chars = input.Select(c => char.IsLetterOrDigit(c) || c == '_' ? c : '_').ToArray();
    var cleaned = new string(chars).Trim('_');
    return string.IsNullOrEmpty(cleaned) ? "x" : cleaned;
}

void PrintProvider(ProviderInfo p) =>
    Console.WriteLine($"Sağlayıcı     : {p.Label}  ->  {p.MigrationsProject}");

// Aktif sağlayıcıyı appsettings.json'dan okur, ortam değişkeni geçersiz kılmasını dikkate alır.
// ef.sh / DependencyInjection.IsSqlServerProvider semantiğini yansıtır.
static ProviderInfo ResolveProvider(string appSettingsPath)
{
    var forced = Environment.GetEnvironmentVariable("ENERGY_DB_PROVIDER");
    if (!string.IsNullOrWhiteSpace(forced))
        return IsSqlServer(forced) ? ProviderInfo.SqlServer : ProviderInfo.PostgreSql;

    try
    {
        using var doc = JsonDocument.Parse(File.ReadAllText(appSettingsPath));
        var root = doc.RootElement;

        var env = root.TryGetProperty("Environment", out var e) ? e.GetString() : "Production";

        string? providerValue = null;
        if (!string.IsNullOrWhiteSpace(env)
            && root.TryGetProperty(env, out var envSection)
            && envSection.TryGetProperty("Database", out var dbSection)
            && dbSection.TryGetProperty("Provider", out var prov))
        {
            providerValue = prov.GetString();
        }

        // Fallback to a top-level Database:Provider if present.
        if (providerValue is null
            && root.TryGetProperty("Database", out var topDb)
            && topDb.TryGetProperty("Provider", out var topProv))
        {
            providerValue = topProv.GetString();
        }

        return IsSqlServer(providerValue) ? ProviderInfo.SqlServer : ProviderInfo.PostgreSql;
    }
    catch
    {
        // appsettings okunamazsa varsayılan olarak PostgreSQL kullan (proje varsayılanı).
        return ProviderInfo.PostgreSql;
    }
}

static bool IsSqlServer(string? provider)
{
    if (string.IsNullOrWhiteSpace(provider)) return false;
    var p = provider.Trim().Replace(" ", string.Empty).ToLowerInvariant();
    return p is "sqlserver" or "mssql" or "sql" or "mssqlserver";
}

// ---------------------------------------------------------------------------
// Sağlayıcı tanımlayıcısı: bir sağlayıcıyı kendine ait migrations projesine eşler.
// ---------------------------------------------------------------------------
internal sealed record ProviderInfo(string Label, string MigrationsProject, string ConfigValue)
{
    public static readonly ProviderInfo PostgreSql =
        new("PostgreSQL", "Energy.Migrations.PostgreSql", "PostgreSql");

    public static readonly ProviderInfo SqlServer =
        new("SQL Server", "Energy.Migrations.SqlServer", "SqlServer");
}

