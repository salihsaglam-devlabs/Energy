using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Commands.CreateContractAmendment;

/// <summary>Yeni ContractAmendment oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateContractAmendmentCommand(CreateContractAmendmentRequest Request)
    : IRequest<BaseResponse<Guid>>;
