using Energy.Application.Modules.Core.AuditLog.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.AuditLog.Queries.GetAuditLogLookup;

/// <summary>
/// <see cref="GetAuditLogLookupQuery"/> handler'ı. <see cref="IAuditLogLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetAuditLogLookupQueryHandler
    : IRequestHandler<GetAuditLogLookupQuery, BaseResponse<IReadOnlyList<AuditLogLookupResponse>>>
{
    private readonly IAuditLogLookupService _lookup;

    public GetAuditLogLookupQueryHandler(IAuditLogLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>> Handle(
        GetAuditLogLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
