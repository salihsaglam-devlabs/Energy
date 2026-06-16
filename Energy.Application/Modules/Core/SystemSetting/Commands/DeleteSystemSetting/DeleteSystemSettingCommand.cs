using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SystemSetting.Commands.DeleteSystemSetting;

/// <summary>SystemSetting kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteSystemSettingCommand(Guid Id) : IRequest<BaseResponse<bool>>;
