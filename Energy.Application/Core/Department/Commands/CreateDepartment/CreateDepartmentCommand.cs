using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Requests;
using MediatR;

namespace Energy.Application.Core.Department.Commands.CreateDepartment;

/// <summary>Yeni Department oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDepartmentCommand(CreateDepartmentRequest Request)
    : IRequest<BaseResponse<Guid>>;
