using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;
using MediatR;

namespace Energy.Application.Core.SystemSetting.Queries.GetSystemSettingList;

/// <summary>Sayfalanmış SystemSetting listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetSystemSettingListQuery(GetSystemSettingListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<SystemSettingListResponse>>>;
