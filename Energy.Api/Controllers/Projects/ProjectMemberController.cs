using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Projects.ProjectMember.Commands.CreateProjectMember;
using Energy.Application.Projects.ProjectMember.Commands.DeleteProjectMember;
using Energy.Application.Projects.ProjectMember.Commands.UpdateProjectMember;
using Energy.Application.Projects.ProjectMember.Queries.GetProjectMemberById;
using Energy.Application.Projects.ProjectMember.Queries.GetProjectMemberList;
using Energy.Application.Projects.ProjectMember.Queries.GetProjectMemberLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Requests;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;

namespace Energy.Api.Controllers.Projects;

/// <summary>
/// ProjectMember uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/project-members")]
public sealed class ProjectMemberController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectMemberController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectMemberListResponse>>>> GetList([FromQuery] GetProjectMemberListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectMemberListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProjectMemberDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectMemberByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectMemberLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProjectMemberRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProjectMemberCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProjectMemberRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProjectMemberCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProjectMemberCommand(id), ct));
}
