using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Core.Company.Commands.CreateCompany;
using Energy.Application.Core.Company.Commands.DeleteCompany;
using Energy.Application.Core.Company.Commands.UpdateCompany;
using Energy.Application.Core.Company.Queries.GetCompanyById;
using Energy.Application.Core.Company.Queries.GetCompanyList;
using Energy.Application.Core.Company.Queries.GetCompanyLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Requests;
using Energy.Shared.Models.V1.Core.Company.Responses;

namespace Energy.Api.Controllers.Core;

/// <summary>
/// Company uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/core/companies")]
public sealed class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<CompanyListResponse>>>> GetList([FromQuery] GetCompanyListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCompanyListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<CompanyDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCompanyByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<CompanyLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCompanyLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateCompanyRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateCompanyCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateCompanyRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCompanyCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteCompanyCommand(id), ct));
}
