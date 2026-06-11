using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Permissions.Commands.SeedDefaultPermissions;

public sealed record SeedDefaultPermissionsCommand : IRequest<BaseResponse<SeedResultResponse>>;
