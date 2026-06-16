using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;
using MediatR;

namespace Energy.Application.Modules.Core.SystemSetting.Commands.CreateSystemSetting;

/// <summary>Yeni SystemSetting oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateSystemSettingCommand(CreateSystemSettingRequest Request)
    : IRequest<BaseResponse<Guid>>;
