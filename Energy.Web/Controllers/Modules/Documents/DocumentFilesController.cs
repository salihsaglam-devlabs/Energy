using Energy.Web.Clients.Modules.Documents.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers.Modules.Documents;

/// <summary>
/// Belge dosya/versiyon yönetimi ekran denetleyicisi. Yükleme (multipart), versiyon
/// geçmişi ve indirme isteklerini API istemcisi üzerinden iletir; dosya sistemine
/// doğrudan dokunmaz.
/// </summary>
[Authorize]
[Route("documents/files")]
public sealed class DocumentFilesController : Controller
{
    private readonly IDocumentFilesApiClient _api;

    public DocumentFilesController(IDocumentFilesApiClient api) => _api = api;

    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Modules/Documents/Files/Index.cshtml");

    [HttpPost("upload")]
    [IgnoreAntiforgeryToken]
    [RequestSizeLimit(104_857_600)]
    public async Task<IActionResult> Upload([FromForm] Guid documentId, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
        {
            return Json(new { isSuccess = false, message = "File is required." });
        }

        await using var stream = file.OpenReadStream();
        var result = await _api.UploadAsync(documentId, stream, file.FileName, file.ContentType, ct);
        return Json(result);
    }

    [HttpGet("versions/{documentId:guid}")]
    public async Task<IActionResult> Versions(Guid documentId, CancellationToken ct)
        => Json((await _api.GetVersionsAsync(documentId, ct)).Data ?? []);

    [HttpGet("download/{versionId:guid}")]
    public async Task<IActionResult> Download(Guid versionId, CancellationToken ct)
    {
        var (content, contentType, statusCode) = await _api.DownloadAsync(versionId, ct);
        if (statusCode >= 400 || content.Length == 0)
        {
            return NotFound();
        }

        return File(content, contentType);
    }
}

