using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.CostCenter.Commands.CreateCostCenter;

/// <summary>Yeni CostCenter oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateCostCenterCommand(CreateCostCenterRequest Request)
    : IRequest<BaseResponse<Guid>>;
