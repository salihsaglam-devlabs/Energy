using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuote.Commands.CreateSupplierQuote;

/// <summary>Yeni SupplierQuote oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateSupplierQuoteCommand(CreateSupplierQuoteRequest Request)
    : IRequest<BaseResponse<Guid>>;
