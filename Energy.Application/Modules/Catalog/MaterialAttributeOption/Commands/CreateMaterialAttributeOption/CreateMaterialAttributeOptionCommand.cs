using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Commands.CreateMaterialAttributeOption;

/// <summary>Yeni MaterialAttributeOption oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMaterialAttributeOptionCommand(CreateMaterialAttributeOptionRequest Request)
    : IRequest<BaseResponse<Guid>>;
