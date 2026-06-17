using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentLookup;

/// <summary>EmployeeSkillAssignment lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetEmployeeSkillAssignmentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>>;
