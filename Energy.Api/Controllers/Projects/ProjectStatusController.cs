using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Projects.ProjectStatus.Commands.CreateProjectStatus;
using Energy.Application.Modules.Projects.ProjectStatus.Commands.DeleteProjectStatus;
using Energy.Application.Modules.Projects.ProjectStatus.Commands.UpdateProjectStatus;
using Energy.Application.Modules.Projects.ProjectStatus.Queries.GetProjectStatusById;
using Energy.Application.Modules.Projects.ProjectStatus.Queries.GetProjectStatusList;
using Energy.Application.Modules.Projects.ProjectStatus.Queries.GetProjectStatusLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;

namespace Energy.Api.Controllers.Projects;

/// <summary>
/// ProjectStatus uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/project-statuses")]
public sealed class ProjectStatusController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectStatusController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectStatusListResponse>>>> GetList([FromQuery] GetProjectStatusListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectStatusListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProjectStatusDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectStatusByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProjectStatusLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectStatusLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProjectStatusRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProjectStatusCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProjectStatusRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProjectStatusCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProjectStatusCommand(id), ct));
}
