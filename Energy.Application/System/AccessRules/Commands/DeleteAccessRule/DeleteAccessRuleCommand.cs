using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.DeleteAccessRule;

public sealed record DeleteAccessRuleCommand(Guid Id)
    : IRequest<BaseResponse<Guid>>;

