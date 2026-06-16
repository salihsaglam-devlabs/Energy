using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Requests;
using MediatR;

namespace Energy.Application.Core.Department.Commands.UpdateDepartment;

/// <summary>Var olan Department kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDepartmentCommand(Guid Id, UpdateDepartmentRequest Request)
    : IRequest<BaseResponse<bool>>;
