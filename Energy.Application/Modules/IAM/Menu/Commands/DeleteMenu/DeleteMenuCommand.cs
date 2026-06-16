using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.Menu.Commands.DeleteMenu;

/// <summary>DeleteMenu</summary>
public sealed record DeleteMenuCommand(Guid Id)
    : IRequest<BaseResponse<bool>>;
