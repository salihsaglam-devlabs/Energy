using Energy.Application.System.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Commands.DeleteAccessRule;

public sealed class DeleteAccessRuleCommandHandler
    : IRequestHandler<DeleteAccessRuleCommand, BaseResponse<Guid>>
{
    private readonly IAccessRuleService _accessRuleService;

    public DeleteAccessRuleCommandHandler(IAccessRuleService accessRuleService)
    {
        _accessRuleService = accessRuleService;
    }

    public async Task<BaseResponse<Guid>> Handle(DeleteAccessRuleCommand request, CancellationToken cancellationToken)
    {
        await _accessRuleService.DeleteAccessRuleAsync(request.Id, cancellationToken);
        return BaseResponse<Guid>.Success(request.Id);
    }
}

