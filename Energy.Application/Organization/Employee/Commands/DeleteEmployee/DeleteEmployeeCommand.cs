using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.Employee.Commands.DeleteEmployee;

/// <summary>Employee kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteEmployeeCommand(Guid Id) : IRequest<BaseResponse<bool>>;
