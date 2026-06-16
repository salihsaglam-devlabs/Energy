using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Projects.ProjectLocation.Commands.CreateProjectLocation;
using Energy.Application.Modules.Projects.ProjectLocation.Commands.DeleteProjectLocation;
using Energy.Application.Modules.Projects.ProjectLocation.Commands.UpdateProjectLocation;
using Energy.Application.Modules.Projects.ProjectLocation.Queries.GetProjectLocationById;
using Energy.Application.Modules.Projects.ProjectLocation.Queries.GetProjectLocationList;
using Energy.Application.Modules.Projects.ProjectLocation.Queries.GetProjectLocationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;

namespace Energy.Api.Controllers.Projects;

/// <summary>
/// ProjectLocation uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/project-locations")]
public sealed class ProjectLocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectLocationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectLocationListResponse>>>> GetList([FromQuery] GetProjectLocationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectLocationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProjectLocationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectLocationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectLocationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProjectLocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProjectLocationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProjectLocationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProjectLocationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProjectLocationCommand(id), ct));
}
