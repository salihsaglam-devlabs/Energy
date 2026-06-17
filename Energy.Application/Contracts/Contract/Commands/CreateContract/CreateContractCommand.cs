using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;
using MediatR;

namespace Energy.Application.Contracts.Contract.Commands.CreateContract;

/// <summary>Yeni Contract oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateContractCommand(CreateContractRequest Request)
    : IRequest<BaseResponse<Guid>>;
