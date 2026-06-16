using Asp.Versioning;
using Energy.Application.Modules.Documents.Files.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers.Modules.Documents.Files;

/// <summary>
/// Belge dosya/versiyon uç noktaları: yeni versiyon yükleme (multipart), versiyon
/// geçmişi ve versiyon indirme. Dosya işlemleri servis soyutlaması üzerinden yürür;
/// controller dosya sistemine doğrudan dokunmaz.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/documents/files")]
public sealed class DocumentFilesController : ControllerBase
{
    private readonly IDocumentFileService _files;

    public DocumentFilesController(IDocumentFileService files) => _files = files;

    /// <summary>Belgeye yeni bir versiyon yükler (multipart/form-data).</summary>
    [HttpPost("upload")]
    [RequestSizeLimit(104_857_600)] // 100 MB
    public async Task<ActionResult<BaseResponse<DocumentVersionFileResponse>>> Upload(
        [FromForm] Guid documentId, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(BaseResponse<DocumentVersionFileResponse>.Failure("File is required."));
        }

        await using var stream = file.OpenReadStream();
        var result = await _files.UploadNewVersionAsync(
            documentId, stream, file.FileName, file.ContentType, file.Length, ct);
        return Ok(result);
    }

    /// <summary>Belgenin versiyon geçmişi (yeniden eskiye).</summary>
    [HttpGet("versions/{documentId:guid}")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>>> Versions(Guid documentId, CancellationToken ct)
        => Ok(await _files.GetVersionsAsync(documentId, ct));

    /// <summary>Bir versiyonun dosya içeriğini indirir.</summary>
    [HttpGet("download/{versionId:guid}")]
    public async Task<IActionResult> Download(Guid versionId, CancellationToken ct)
    {
        var download = await _files.GetVersionContentAsync(versionId, ct);
        if (download is null)
        {
            return NotFound();
        }

        return File(download.Content, download.ContentType, download.FileName);
    }
}

