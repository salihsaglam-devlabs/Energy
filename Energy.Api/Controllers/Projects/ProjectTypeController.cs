using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Projects.ProjectType.Commands.CreateProjectType;
using Energy.Application.Modules.Projects.ProjectType.Commands.DeleteProjectType;
using Energy.Application.Modules.Projects.ProjectType.Commands.UpdateProjectType;
using Energy.Application.Modules.Projects.ProjectType.Queries.GetProjectTypeById;
using Energy.Application.Modules.Projects.ProjectType.Queries.GetProjectTypeList;
using Energy.Application.Modules.Projects.ProjectType.Queries.GetProjectTypeLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Requests;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;

namespace Energy.Api.Controllers.Projects;

/// <summary>
/// ProjectType uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/project-types")]
public sealed class ProjectTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectTypeController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectTypeListResponse>>>> GetList([FromQuery] GetProjectTypeListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectTypeListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProjectTypeDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectTypeByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProjectTypeLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectTypeLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProjectTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProjectTypeCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProjectTypeRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProjectTypeCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProjectTypeCommand(id), ct));
}
