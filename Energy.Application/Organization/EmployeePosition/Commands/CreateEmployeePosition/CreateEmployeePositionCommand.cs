using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;
using MediatR;

namespace Energy.Application.Organization.EmployeePosition.Commands.CreateEmployeePosition;

/// <summary>Yeni EmployeePosition oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateEmployeePositionCommand(CreateEmployeePositionRequest Request)
    : IRequest<BaseResponse<Guid>>;
