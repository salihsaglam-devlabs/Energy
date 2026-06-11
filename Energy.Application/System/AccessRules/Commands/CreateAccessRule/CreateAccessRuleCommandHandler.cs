using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.CreateAccessRule;

public sealed class CreateAccessRuleCommandHandler
    : IRequestHandler<CreateAccessRuleCommand, BaseResponse<AccessRuleResponse>>
{
    private readonly IAccessRuleService _accessRuleService;

    public CreateAccessRuleCommandHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<AccessRuleResponse>> Handle(
        CreateAccessRuleCommand request,
        CancellationToken cancellationToken)
    {
        var data = await _accessRuleService.CreateAccessRuleAsync(request.Request, cancellationToken);
        return BaseResponse<AccessRuleResponse>.Success(data);
    }
}

