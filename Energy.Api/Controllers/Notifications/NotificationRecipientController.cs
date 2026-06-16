using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Application.Modules.Notifications.NotificationRecipient.Commands.CreateNotificationRecipient;
using Energy.Application.Modules.Notifications.NotificationRecipient.Commands.DeleteNotificationRecipient;
using Energy.Application.Modules.Notifications.NotificationRecipient.Commands.UpdateNotificationRecipient;
using Energy.Application.Modules.Notifications.NotificationRecipient.Queries.GetNotificationRecipientById;
using Energy.Application.Modules.Notifications.NotificationRecipient.Queries.GetNotificationRecipientList;
using Energy.Application.Modules.Notifications.NotificationRecipient.Queries.GetNotificationRecipientLookup;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;

namespace Energy.Api.Controllers.Notifications;

/// <summary>
/// NotificationRecipient uç noktaları (liste, detay, lookup, create, update, delete).
/// Controller iş mantığı içermez; her istek ilgili Command/Query'ye map edilip
/// <see cref="IMediator"/> üzerinden Application use-case'ine yönlendirilir.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notifications/notification-recipients")]
public sealed class NotificationRecipientController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationRecipientController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>Sayfalanmış liste.</summary>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>>> GetList([FromQuery] GetNotificationRecipientListRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationRecipientListQuery(request), ct));

    /// <summary>Kimliğe göre detay.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<NotificationRecipientDetailResponse>>> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationRecipientByIdQuery(id), ct));

    /// <summary>Lookup listesi.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>>> Lookup([FromQuery] string? search, [FromQuery] bool activeOnly, CancellationToken ct)
        => Ok(await _mediator.Send(new GetNotificationRecipientLookupQuery(search, activeOnly), ct));

    /// <summary>Yeni kayıt oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Create(CreateNotificationRecipientRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateNotificationRecipientCommand(request), ct));

    /// <summary>Var olan kaydı günceller.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Update(Guid id, UpdateNotificationRecipientRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateNotificationRecipientCommand(id, request), ct));

    /// <summary>Kaydı (soft-delete) siler.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> Delete(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteNotificationRecipientCommand(id), ct));
}
