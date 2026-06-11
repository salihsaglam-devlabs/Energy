using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenuTree;

public sealed record GetMenuTreeQuery : IRequest<BaseResponse<IReadOnlyList<MenuResponse>>>;

