using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Requests;
using MediatR;

namespace Energy.Application.Modules.Catalog.Material.Commands.CreateMaterial;

/// <summary>Yeni Material oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMaterialCommand(CreateMaterialRequest Request)
    : IRequest<BaseResponse<Guid>>;
