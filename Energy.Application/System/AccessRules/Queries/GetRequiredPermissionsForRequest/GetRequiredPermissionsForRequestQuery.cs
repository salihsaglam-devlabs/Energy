using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.System.AccessRules.Queries.GetRequiredPermissionsForRequest;

public sealed record GetRequiredPermissionsForRequestQuery(
    string Scope,
    string Path,
    string? HttpMethod)
    : IRequest<BaseResponse<IReadOnlyList<string>>>;

