using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Documents.DocumentPermission.Commands.CreateDocumentPermission;
using Energy.Application.Documents.DocumentPermission.Commands.DeleteDocumentPermission;
using Energy.Application.Documents.DocumentPermission.Commands.UpdateDocumentPermission;
using Energy.Application.Documents.DocumentPermission.Queries.GetDocumentPermissionById;
using Energy.Application.Documents.DocumentPermission.Queries.GetDocumentPermissionList;
using Energy.Application.Documents.DocumentPermission.Queries.GetDocumentPermissionLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;

namespace Energy.Api.Controllers.Documents;

/// <summary>
/// DocumentPermission uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/documents/document-permissions")]
public sealed class DocumentPermissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentPermissionController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>>> GetList([FromQuery] GetDocumentPermissionListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentPermissionListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DocumentPermissionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentPermissionByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentPermissionLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDocumentPermissionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDocumentPermissionCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDocumentPermissionRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDocumentPermissionCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDocumentPermissionCommand(id), ct));
}
