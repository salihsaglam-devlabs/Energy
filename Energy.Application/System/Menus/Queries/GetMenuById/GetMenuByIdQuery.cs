using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Queries.GetMenuById;

public sealed record GetMenuByIdQuery(Guid Id) : IRequest<BaseResponse<MenuResponse>>;
