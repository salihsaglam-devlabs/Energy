using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.Menu.Commands.CreateMenu;

/// <summary>CreateMenu</summary>
public sealed record CreateMenuCommand(CreateMenuRequest Request)
    : IRequest<BaseResponse<MenuResponse>>;
