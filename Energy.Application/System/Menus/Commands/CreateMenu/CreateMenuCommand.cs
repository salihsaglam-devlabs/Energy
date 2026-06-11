using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.CreateMenu;

public sealed record CreateMenuCommand(CreateMenuRequest Request)
    : IRequest<BaseResponse<MenuResponse>>;
