using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Projects.ProjectPhas.Commands.CreateProjectPhas;
using Energy.Application.Projects.ProjectPhas.Commands.DeleteProjectPhas;
using Energy.Application.Projects.ProjectPhas.Commands.UpdateProjectPhas;
using Energy.Application.Projects.ProjectPhas.Queries.GetProjectPhasById;
using Energy.Application.Projects.ProjectPhas.Queries.GetProjectPhasList;
using Energy.Application.Projects.ProjectPhas.Queries.GetProjectPhasLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

namespace Energy.Api.Controllers.Projects;

/// <summary>
/// ProjectPhas uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/project-phases")]
public sealed class ProjectPhasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectPhasController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectPhasListResponse>>>> GetList([FromQuery] GetProjectPhasListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectPhasListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProjectPhasDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectPhasByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectPhasLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProjectPhasRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProjectPhasCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProjectPhasRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProjectPhasCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProjectPhasCommand(id), ct));
}
