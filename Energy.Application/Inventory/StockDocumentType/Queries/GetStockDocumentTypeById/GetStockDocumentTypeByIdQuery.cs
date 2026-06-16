using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentType.Queries.GetStockDocumentTypeById;

/// <summary>Kimliğe göre StockDocumentType detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockDocumentTypeByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockDocumentTypeDetailResponse>>;
