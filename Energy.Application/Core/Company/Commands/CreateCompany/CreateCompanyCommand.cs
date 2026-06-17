using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Requests;
using MediatR;

namespace Energy.Application.Core.Company.Commands.CreateCompany;

/// <summary>Yeni Company oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateCompanyCommand(CreateCompanyRequest Request)
    : IRequest<BaseResponse<Guid>>;
