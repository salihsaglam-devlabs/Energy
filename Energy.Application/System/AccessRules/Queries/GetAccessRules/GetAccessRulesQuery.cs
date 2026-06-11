using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetAccessRules;

public sealed class GetAccessRulesQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<AccessRuleResponse>>>;

