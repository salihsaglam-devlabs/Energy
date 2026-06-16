using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Department.Queries.GetDepartmentById;

/// <summary>Kimliğe göre Department detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetDepartmentByIdQuery(Guid Id)
    : IRequest<BaseResponse<DepartmentDetailResponse>>;
