using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.AuditLog.Queries.GetAuditLogLookup;

/// <summary>AuditLog lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetAuditLogLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>>;
