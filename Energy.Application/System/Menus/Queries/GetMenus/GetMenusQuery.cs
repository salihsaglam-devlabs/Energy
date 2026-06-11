using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenus;

public sealed class GetMenusQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<MenuResponse>>>;
