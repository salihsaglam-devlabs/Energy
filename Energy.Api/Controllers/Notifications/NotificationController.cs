using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Notifications.Notification.Commands.CreateNotification;
using Energy.Application.Notifications.Notification.Commands.DeleteNotification;
using Energy.Application.Notifications.Notification.Commands.UpdateNotification;
using Energy.Application.Notifications.Notification.Queries.GetNotificationById;
using Energy.Application.Notifications.Notification.Queries.GetNotificationList;
using Energy.Application.Notifications.Notification.Queries.GetNotificationLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;

namespace Energy.Api.Controllers.Notifications;

/// <summary>
/// Notification uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notifications/notifications")]
public sealed class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<NotificationListResponse>>>> GetList([FromQuery] GetNotificationListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<NotificationDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<NotificationLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateNotificationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateNotificationCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateNotificationRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateNotificationCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteNotificationCommand(id), ct));
}
