using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineById;

/// <summary>Kimliğe göre SupplierQuoteLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetSupplierQuoteLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<SupplierQuoteLineDetailResponse>>;
