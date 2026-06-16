using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Documents.Files.Commands.UploadDocumentVersion;
using Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersionContent;
using Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersions;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;

namespace Energy.Api.Controllers.Documents.Files;

/// <summary>
/// Belge dosya/versiyon uç noktaları: yeni versiyon yükleme (multipart), versiyon
/// geçmişi ve versiyon indirme. İş mantığı MediatR handler'ları + dosya servisi
/// soyutlaması üzerinden yürür; controller dosya sistemine doğrudan dokunmaz.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/documents/files")]
public sealed class DocumentFilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentFilesController(IMediator mediator)
        => _mediator = mediator;

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

        using var buffer = new MemoryStream();
        await file.CopyToAsync(buffer, ct);
        return Ok(await _mediator.Send(new UploadDocumentVersionCommand(
            documentId, buffer.ToArray(), file.FileName, file.ContentType, file.Length), ct));
    }

    /// <summary>Belgenin versiyon geçmişi (yeniden eskiye).</summary>
    [HttpGet("versions/{documentId:guid}")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>>> Versions(Guid documentId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentVersionsQuery(documentId), ct));

    /// <summary>Bir versiyonun dosya içeriğini indirir.</summary>
    [HttpGet("download/{versionId:guid}")]
    public async Task<IActionResult> Download(Guid versionId, CancellationToken ct)
    {
        var download = await _mediator.Send(new GetDocumentVersionContentQuery(versionId), ct);
        return download is null ? NotFound() : File(download.Content, download.ContentType, download.FileName);
    }
}
