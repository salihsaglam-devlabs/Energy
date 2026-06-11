using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetAccessRuleById;

public sealed record GetAccessRuleByIdQuery(Guid Id)
    : IRequest<BaseResponse<AccessRuleResponse>>;

