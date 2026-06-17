using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Requests;
using MediatR;

namespace Energy.Application.Core.Company.Commands.UpdateCompany;

/// <summary>Var olan Company kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateCompanyCommand(Guid Id, UpdateCompanyRequest Request)
    : IRequest<BaseResponse<bool>>;
