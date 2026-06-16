using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.Brand.Commands.UpdateBrand;

/// <summary>Var olan Brand kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBrandCommand(Guid Id, UpdateBrandRequest Request)
    : IRequest<BaseResponse<bool>>;
