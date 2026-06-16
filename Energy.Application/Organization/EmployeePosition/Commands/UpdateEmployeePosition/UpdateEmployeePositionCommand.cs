using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;
using MediatR;

namespace Energy.Application.Organization.EmployeePosition.Commands.UpdateEmployeePosition;

/// <summary>Var olan EmployeePosition kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateEmployeePositionCommand(Guid Id, UpdateEmployeePositionRequest Request)
    : IRequest<BaseResponse<bool>>;
