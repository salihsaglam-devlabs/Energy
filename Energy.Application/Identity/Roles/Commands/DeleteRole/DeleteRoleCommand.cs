using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid Id) : IRequest<BaseResponse<Guid>>;
