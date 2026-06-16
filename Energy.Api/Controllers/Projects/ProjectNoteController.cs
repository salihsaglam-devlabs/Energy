using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Projects.ProjectNote.Commands.CreateProjectNote;
using Energy.Application.Projects.ProjectNote.Commands.DeleteProjectNote;
using Energy.Application.Projects.ProjectNote.Commands.UpdateProjectNote;
using Energy.Application.Projects.ProjectNote.Queries.GetProjectNoteById;
using Energy.Application.Projects.ProjectNote.Queries.GetProjectNoteList;
using Energy.Application.Projects.ProjectNote.Queries.GetProjectNoteLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;

namespace Energy.Api.Controllers.Projects;

/// <summary>
/// ProjectNote uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/project-notes")]
public sealed class ProjectNoteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectNoteController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<ProjectNoteListResponse>>>> GetList([FromQuery] GetProjectNoteListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectNoteListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ProjectNoteDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectNoteByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectNoteLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateProjectNoteRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateProjectNoteCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateProjectNoteRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProjectNoteCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteProjectNoteCommand(id), ct));
}
