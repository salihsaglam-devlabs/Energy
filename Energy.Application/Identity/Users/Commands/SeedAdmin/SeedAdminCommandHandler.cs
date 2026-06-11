using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.SeedAdmin;

public sealed class SeedAdminCommandHandler
    : IRequestHandler<SeedAdminCommand, BaseResponse<SeedAdminResponse>>
{
    private readonly IUserService _userService;

    public SeedAdminCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<SeedAdminResponse>> Handle(
        SeedAdminCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.SeedAdminAsync(cancellationToken);
        return BaseResponse<SeedAdminResponse>.Success(result);
    }
}
