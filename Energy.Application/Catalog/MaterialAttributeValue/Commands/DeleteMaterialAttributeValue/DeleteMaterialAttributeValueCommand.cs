using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeValue.Commands.DeleteMaterialAttributeValue;

/// <summary>MaterialAttributeValue kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMaterialAttributeValueCommand(Guid Id) : IRequest<BaseResponse<bool>>;
