using Energy.Application.Common.Storage;
using Microsoft.Extensions.Configuration;

namespace Energy.Infrastructure.Common.Storage;

/// <summary>
/// Yerel disk tabanlı dosya saklama. Kök dizin <c>Storage:DocumentsPath</c>
/// yapılandırmasından okunur; tanımlı değilse uygulama tabanı altında
/// <c>App_Data/documents</c> kullanılır. Dosyalar tarih bazlı alt klasörlere,
/// çakışmayı önlemek için GUID önekiyle yazılır.
/// </summary>
public sealed class LocalFileStorage : IFileStorage
{
    private readonly string _root;

    public LocalFileStorage(IConfiguration configuration)
    {
        var configured = configuration["Storage:DocumentsPath"];
        _root = string.IsNullOrWhiteSpace(configured)
            ? Path.Combine(AppContext.BaseDirectory, "App_Data", "documents")
            : configured;
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveAsync(Stream content, string fileName, CancellationToken ct = default)
    {
        var safeName = Path.GetFileName(fileName);
        var subDir = DateTime.UtcNow.ToString("yyyy/MM");
        var relativeDir = Path.Combine(subDir);
        var absoluteDir = Path.Combine(_root, relativeDir);
        Directory.CreateDirectory(absoluteDir);

        var storedName = $"{Guid.NewGuid():N}_{safeName}";
        var relativePath = Path.Combine(relativeDir, storedName).Replace('\\', '/');
        var absolutePath = Path.Combine(_root, relativePath);

        await using (var target = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
        {
            await content.CopyToAsync(target, ct);
        }

        return relativePath;
    }

    public Task<Stream?> OpenAsync(string relativePath, CancellationToken ct = default)
    {
        var absolutePath = ResolveSafe(relativePath);
        if (absolutePath is null || !File.Exists(absolutePath))
        {
            return Task.FromResult<Stream?>(null);
        }

        Stream stream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult<Stream?>(stream);
    }

    public Task DeleteAsync(string relativePath, CancellationToken ct = default)
    {
        var absolutePath = ResolveSafe(relativePath);
        if (absolutePath is not null && File.Exists(absolutePath))
        {
            File.Delete(absolutePath);
        }

        return Task.CompletedTask;
    }

    /// <summary>Göreli yolu köke göre çözer ve kök dışına çıkışı (path traversal) engeller.</summary>
    private string? ResolveSafe(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return null;
        }

        var absolutePath = Path.GetFullPath(Path.Combine(_root, relativePath));
        var rootFull = Path.GetFullPath(_root);
        return absolutePath.StartsWith(rootFull, StringComparison.Ordinal) ? absolutePath : null;
    }
}

