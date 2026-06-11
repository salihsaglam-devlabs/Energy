using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.Menus.Commands.DeleteMenu;

public sealed record DeleteMenuCommand(Guid Id) : IRequest<BaseResponse<Guid>>;
