using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteById;

/// <summary>Kimliğe göre SupplierQuote detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetSupplierQuoteByIdQuery(Guid Id)
    : IRequest<BaseResponse<SupplierQuoteDetailResponse>>;
