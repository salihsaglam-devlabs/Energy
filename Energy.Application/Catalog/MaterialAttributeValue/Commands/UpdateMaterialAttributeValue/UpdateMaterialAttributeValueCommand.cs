using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeValue.Commands.UpdateMaterialAttributeValue;

/// <summary>Var olan MaterialAttributeValue kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMaterialAttributeValueCommand(Guid Id, UpdateMaterialAttributeValueRequest Request)
    : IRequest<BaseResponse<bool>>;
