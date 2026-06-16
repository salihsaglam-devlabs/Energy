using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Requests;
using MediatR;

namespace Energy.Application.Modules.Organization.Employee.Commands.CreateEmployee;

/// <summary>Yeni Employee oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateEmployeeCommand(CreateEmployeeRequest Request)
    : IRequest<BaseResponse<Guid>>;
