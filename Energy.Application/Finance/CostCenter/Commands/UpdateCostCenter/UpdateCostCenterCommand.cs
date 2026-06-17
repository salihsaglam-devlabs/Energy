using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;
using MediatR;

namespace Energy.Application.Finance.CostCenter.Commands.UpdateCostCenter;

/// <summary>Var olan CostCenter kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateCostCenterCommand(Guid Id, UpdateCostCenterRequest Request)
    : IRequest<BaseResponse<bool>>;
