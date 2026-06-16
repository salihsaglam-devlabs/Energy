using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;
using MediatR;

namespace Energy.Application.Contracts.ContractLine.Commands.CreateContractLine;

/// <summary>Yeni ContractLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateContractLineCommand(CreateContractLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
