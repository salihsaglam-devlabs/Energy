using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetAccessRuleById;

public sealed class GetAccessRuleByIdQueryHandler
    : IRequestHandler<GetAccessRuleByIdQuery, BaseResponse<AccessRuleResponse>>
{
    private readonly IAccessRuleService _accessRuleService;

    public GetAccessRuleByIdQueryHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<AccessRuleResponse>> Handle(
        GetAccessRuleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _accessRuleService.GetAccessRuleByIdAsync(request.Id, cancellationToken);
        return BaseResponse<AccessRuleResponse>.Success(data);
    }
}

