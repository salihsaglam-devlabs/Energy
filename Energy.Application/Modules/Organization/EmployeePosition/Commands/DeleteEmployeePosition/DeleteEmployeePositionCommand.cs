using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeePosition.Commands.DeleteEmployeePosition;

/// <summary>EmployeePosition kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteEmployeePositionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
