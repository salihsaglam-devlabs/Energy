using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.UpdateMenu;

public sealed record UpdateMenuCommand(Guid Id, UpdateMenuRequest Request)
    : IRequest<BaseResponse<MenuResponse>>;
