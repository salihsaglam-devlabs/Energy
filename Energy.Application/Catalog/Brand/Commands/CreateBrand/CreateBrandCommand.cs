using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;
using MediatR;

namespace Energy.Application.Catalog.Brand.Commands.CreateBrand;

/// <summary>Yeni Brand oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateBrandCommand(CreateBrandRequest Request)
    : IRequest<BaseResponse<Guid>>;
