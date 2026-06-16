using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Notifications.NotificationPreference.Commands.CreateNotificationPreference;
using Energy.Application.Modules.Notifications.NotificationPreference.Commands.DeleteNotificationPreference;
using Energy.Application.Modules.Notifications.NotificationPreference.Commands.UpdateNotificationPreference;
using Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceById;
using Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceList;
using Energy.Application.Modules.Notifications.NotificationPreference.Queries.GetNotificationPreferenceLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;

namespace Energy.Api.Controllers.Notifications;

/// <summary>
/// NotificationPreference uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notifications/notification-preferences")]
public sealed class NotificationPreferenceController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationPreferenceController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>>> GetList([FromQuery] GetNotificationPreferenceListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationPreferenceListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<NotificationPreferenceDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationPreferenceByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationPreferenceLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateNotificationPreferenceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateNotificationPreferenceCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateNotificationPreferenceRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateNotificationPreferenceCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteNotificationPreferenceCommand(id), ct));
}
