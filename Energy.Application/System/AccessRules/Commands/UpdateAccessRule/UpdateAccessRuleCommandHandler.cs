using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.UpdateAccessRule;

public sealed class UpdateAccessRuleCommandHandler
    : IRequestHandler<UpdateAccessRuleCommand, BaseResponse<AccessRuleResponse>>
{
    private readonly IAccessRuleService _accessRuleService;

    public UpdateAccessRuleCommandHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<AccessRuleResponse>> Handle(
        UpdateAccessRuleCommand request,
        CancellationToken cancellationToken)
    {
        var data = await _accessRuleService.UpdateAccessRuleAsync(request.Id, request.Request, cancellationToken);
        return BaseResponse<AccessRuleResponse>.Success(data);
    }
}

