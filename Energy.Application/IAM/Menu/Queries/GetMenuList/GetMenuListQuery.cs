using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.IAM.Menu.Queries.GetMenuList;

/// <summary>GetMenuList</summary>
public sealed record GetMenuListQuery(PaginatedRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MenuResponse>>>;
