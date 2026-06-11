using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.UpdateAccessRule;

public sealed record UpdateAccessRuleCommand(Guid Id, UpdateAccessRuleRequest Request)
    : IRequest<BaseResponse<AccessRuleResponse>>;

