using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Documents.DocumentPermission.Services;
using Energy.Application.Modules.Documents.DocumentPermission.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;

namespace Energy.Api.Controllers.Modules.Documents;

/// <summary>DocumentPermission uç noktaları (liste, detay, lookup, create, update, delete).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/documents/document-permissions")]
public sealed class DocumentPermissionController : ControllerBase
{
    private readonly IDocumentPermissionService _service;
    private readonly IDocumentPermissionLookupService _lookup;

    public DocumentPermissionController(IDocumentPermissionService service, IDocumentPermissionLookupService lookup)
    {
        _service = service;
        _lookup = lookup;
    }

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>>> GetList([FromQuery] GetDocumentPermissionListRequest request, CancellationToken ct)
        => Ok(await _service.GetListAsync(request, ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<DocumentPermissionDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _lookup.GetLookupAsync(search, activeOnly, ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateDocumentPermissionRequest request, CancellationToken ct)
        => Ok(await _service.CreateAsync(request, ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateDocumentPermissionRequest request, CancellationToken ct)
        => Ok(await _service.UpdateAsync(id, request, ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _service.DeleteAsync(id, ct));
}
