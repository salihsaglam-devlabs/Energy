using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Requests;
using MediatR;

namespace Energy.Application.Modules.Organization.Employee.Commands.UpdateEmployee;

/// <summary>Var olan Employee kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateEmployeeCommand(Guid Id, UpdateEmployeeRequest Request)
    : IRequest<BaseResponse<bool>>;
