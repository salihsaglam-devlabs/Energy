using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Documents.DocumentRelation.Commands.CreateDocumentRelation;
using Energy.Application.Modules.Documents.DocumentRelation.Commands.DeleteDocumentRelation;
using Energy.Application.Modules.Documents.DocumentRelation.Commands.UpdateDocumentRelation;
using Energy.Application.Modules.Documents.DocumentRelation.Queries.GetDocumentRelationById;
using Energy.Application.Modules.Documents.DocumentRelation.Queries.GetDocumentRelationList;
using Energy.Application.Modules.Documents.DocumentRelation.Queries.GetDocumentRelationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;

namespace Energy.Api.Controllers.Documents;

/// <summary>
/// DocumentRelation uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/documents/document-relations")]
public sealed class DocumentRelationController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentRelationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DocumentRelationListResponse>>>> GetList([FromQuery] GetDocumentRelationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentRelationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DocumentRelationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentRelationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDocumentRelationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDocumentRelationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateDocumentRelationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDocumentRelationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateDocumentRelationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteDocumentRelationCommand(id), ct));
}
