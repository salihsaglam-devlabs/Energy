using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;
using MediatR;

namespace Energy.Application.Modules.Core.SystemSetting.Commands.UpdateSystemSetting;

/// <summary>Var olan SystemSetting kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateSystemSettingCommand(Guid Id, UpdateSystemSettingRequest Request)
    : IRequest<BaseResponse<bool>>;
