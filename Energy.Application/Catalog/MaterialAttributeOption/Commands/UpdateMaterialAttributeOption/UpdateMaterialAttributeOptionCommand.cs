using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeOption.Commands.UpdateMaterialAttributeOption;

/// <summary>Var olan MaterialAttributeOption kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMaterialAttributeOptionCommand(Guid Id, UpdateMaterialAttributeOptionRequest Request)
    : IRequest<BaseResponse<bool>>;
