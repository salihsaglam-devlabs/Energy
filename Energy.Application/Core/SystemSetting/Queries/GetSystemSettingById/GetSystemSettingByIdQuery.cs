using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;
using MediatR;

namespace Energy.Application.Core.SystemSetting.Queries.GetSystemSettingById;

/// <summary>Kimliğe göre SystemSetting detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetSystemSettingByIdQuery(Guid Id)
    : IRequest<BaseResponse<SystemSettingDetailResponse>>;
