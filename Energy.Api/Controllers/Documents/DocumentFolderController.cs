using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Documents.DocumentFolder.Commands.CreateDocumentFolder;
using Energy.Application.Modules.Documents.DocumentFolder.Commands.DeleteDocumentFolder;
using Energy.Application.Modules.Documents.DocumentFolder.Commands.UpdateDocumentFolder;
using Energy.Application.Modules.Documents.DocumentFolder.Queries.GetDocumentFolderById;
using Energy.Application.Modules.Documents.DocumentFolder.Queries.GetDocumentFolderList;
using Energy.Application.Modules.Documents.DocumentFolder.Queries.GetDocumentFolderLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;

namespace Energy.Api.Controllers.Documents;

/// <summary>
/// DocumentFolder uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/documents/document-folders")]
public sealed class DocumentFolderController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentFolderController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DocumentFolderListResponse>>>> GetList([FromQuery] GetDocumentFolderListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentFolderListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DocumentFolderDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentFolderByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DocumentFolderLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentFolderLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDocumentFolderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDocumentFolderCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDocumentFolderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDocumentFolderCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDocumentFolderCommand(id), ct));
}
