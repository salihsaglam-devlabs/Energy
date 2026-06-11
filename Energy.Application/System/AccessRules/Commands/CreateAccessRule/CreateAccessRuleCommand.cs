using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.CreateAccessRule;

public sealed record CreateAccessRuleCommand(CreateAccessRuleRequest Request)
    : IRequest<BaseResponse<AccessRuleResponse>>;

