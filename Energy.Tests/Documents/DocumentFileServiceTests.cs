using Energy.Shared.Common;
using Energy.Application.Common.Storage;
using Energy.Domain.Documents;
using Energy.Infrastructure.Documents.Files;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Xunit;

namespace Energy.Tests.Documents;

/// <summary>
/// DocumentFileService dosya/versiyon yönetimi testleri: yeni versiyon yükleme,
/// CurrentVersionNo artışı, versiyon listesi ve indirme (fake saklama + InMemory).
/// </summary>
public sealed class DocumentFileServiceTests
{
    /// <summary>Belleğe yazan basit IFileStorage sahtesi.</summary>
    private sealed class FakeFileStorage : IFileStorage
    {
        private readonly Dictionary<string, byte[]> _files = new();
        private int _seq;

        public async Task<string> SaveAsync(Stream content, string fileName, CancellationToken ct = default)
        {
            using var ms = new MemoryStream();
            await content.CopyToAsync(ms, ct);
            var path = $"mem/{_seq++}_{fileName}";
            _files[path] = ms.ToArray();
            return path;
        }

        public Task<Stream?> OpenAsync(string relativePath, CancellationToken ct = default)
            => Task.FromResult<Stream?>(_files.TryGetValue(relativePath, out var bytes) ? new MemoryStream(bytes) : null);

        public Task DeleteAsync(string relativePath, CancellationToken ct = default)
        {
            _files.Remove(relativePath);
            return Task.CompletedTask;
        }
    }

    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task Upload_Then_List_And_Download_RoundTrips()
    {
        await using var db = NewContext();
        var documentId = Guid.NewGuid();
        db.Documents.Add(new Document { Id = documentId, Name = "Spec", Status = Energy.Shared.Common.DocumentStatus.Approved, CurrentVersionNo = 0 });
        await db.SaveChangesAsync();

        var service = new DocumentFileService(db, new FakeFileStorage());
        var payload = Encoding.UTF8.GetBytes("hello world");

        var uploaded = await service.UploadNewVersionAsync(documentId, new MemoryStream(payload), "spec.txt", "text/plain", payload.Length);
        Assert.True(uploaded.IsSuccess);
        Assert.Equal(1, uploaded.Data!.VersionNo);

        var doc = await db.Documents.FirstAsync(d => d.Id == documentId);
        Assert.Equal(1, doc.CurrentVersionNo);

        var second = await service.UploadNewVersionAsync(documentId, new MemoryStream(payload), "spec2.txt", "text/plain", payload.Length);
        Assert.True(second.IsSuccess);
        Assert.Equal(2, second.Data!.VersionNo);

        var versions = await service.GetVersionsAsync(documentId);
        Assert.True(versions.IsSuccess);
        Assert.Equal(2, versions.Data!.Count);
        Assert.Equal(2, versions.Data![0].VersionNo); // newest first

        var download = await service.GetVersionContentAsync(uploaded.Data!.Id);
        Assert.NotNull(download);
        using var reader = new StreamReader(download!.Content);
        Assert.Equal("hello world", await reader.ReadToEndAsync());
    }

    [Fact]
    public async Task Upload_UnknownDocument_ReturnsFailure()
    {
        await using var db = NewContext();
        var service = new DocumentFileService(db, new FakeFileStorage());
        var result = await service.UploadNewVersionAsync(Guid.NewGuid(), new MemoryStream([1, 2, 3]), "x.bin", null, 3);
        Assert.False(result.IsSuccess);
    }
}

