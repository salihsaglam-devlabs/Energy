using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Responses;
using MediatR;

namespace Energy.Application.Core.Department.Queries.GetDepartmentLookup;

/// <summary>Department lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDepartmentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DepartmentLookupResponse>>>;
