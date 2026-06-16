using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.Employee.Queries.GetEmployeeLookup;

/// <summary>Employee lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetEmployeeLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>>;
