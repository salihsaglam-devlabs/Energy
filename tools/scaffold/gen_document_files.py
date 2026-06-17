import os
root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
appf = os.path.join(root, "Energy.Application", "Modules", "Documents", "Files")
apif = os.path.join(root, "Energy.Api", "Controllers", "Documents", "Files")

files = {}

files[os.path.join(appf, "Commands", "UploadDocumentVersion", "UploadDocumentVersionCommand.cs")] = """using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Commands.UploadDocumentVersion;

/// <summary>Belgeye yeni bir dosya versiyonu yükleme use-case'i.</summary>
public sealed record UploadDocumentVersionCommand(
    Guid DocumentId, byte[] Content, string FileName, string ContentType, long Length)
    : IRequest<BaseResponse<DocumentVersionFileResponse>>;
"""

files[os.path.join(appf, "Commands", "UploadDocumentVersion", "UploadDocumentVersionCommandHandler.cs")] = """using Energy.Application.Modules.Documents.Files.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Commands.UploadDocumentVersion;

/// <summary><see cref="UploadDocumentVersionCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class UploadDocumentVersionCommandHandler
    : IRequestHandler<UploadDocumentVersionCommand, BaseResponse<DocumentVersionFileResponse>>
{
    private readonly IDocumentFileService _files;

    public UploadDocumentVersionCommandHandler(IDocumentFileService files)
        => _files = files;

    public async Task<BaseResponse<DocumentVersionFileResponse>> Handle(
        UploadDocumentVersionCommand request, CancellationToken ct)
    {
        using var stream = new MemoryStream(request.Content);
        return await _files.UploadNewVersionAsync(
            request.DocumentId, stream, request.FileName, request.ContentType, request.Length, ct);
    }
}
"""

files[os.path.join(appf, "Queries", "GetDocumentVersions", "GetDocumentVersionsQuery.cs")] = """using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersions;

/// <summary>Bir belgenin dosya versiyon geçmişini getiren sorgu.</summary>
public sealed record GetDocumentVersionsQuery(Guid DocumentId)
    : IRequest<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>>;
"""

files[os.path.join(appf, "Queries", "GetDocumentVersions", "GetDocumentVersionsQueryHandler.cs")] = """using Energy.Application.Modules.Documents.Files.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersions;

/// <summary><see cref="GetDocumentVersionsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetDocumentVersionsQueryHandler
    : IRequestHandler<GetDocumentVersionsQuery, BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>>
{
    private readonly IDocumentFileService _files;

    public GetDocumentVersionsQueryHandler(IDocumentFileService files)
        => _files = files;

    public Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> Handle(
        GetDocumentVersionsQuery request, CancellationToken ct)
        => _files.GetVersionsAsync(request.DocumentId, ct);
}
"""

files[os.path.join(appf, "Queries", "GetDocumentVersionContent", "GetDocumentVersionContentQuery.cs")] = """using Energy.Application.Modules.Documents.Files.Services;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersionContent;

/// <summary>Bir versiyonun indirilebilir dosya içeriğini getiren sorgu.</summary>
public sealed record GetDocumentVersionContentQuery(Guid VersionId)
    : IRequest<DocumentDownload?>;
"""

files[os.path.join(appf, "Queries", "GetDocumentVersionContent", "GetDocumentVersionContentQueryHandler.cs")] = """using Energy.Application.Modules.Documents.Files.Services;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersionContent;

/// <summary><see cref="GetDocumentVersionContentQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetDocumentVersionContentQueryHandler
    : IRequestHandler<GetDocumentVersionContentQuery, DocumentDownload?>
{
    private readonly IDocumentFileService _files;

    public GetDocumentVersionContentQueryHandler(IDocumentFileService files)
        => _files = files;

    public Task<DocumentDownload?> Handle(GetDocumentVersionContentQuery request, CancellationToken ct)
        => _files.GetVersionContentAsync(request.VersionId, ct);
}
"""

files[os.path.join(apif, "DocumentFilesController.cs")] = """using Asp.Versioning;
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
"""

for path, content in files.items():
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)
print(f"wrote {len(files)} files")

