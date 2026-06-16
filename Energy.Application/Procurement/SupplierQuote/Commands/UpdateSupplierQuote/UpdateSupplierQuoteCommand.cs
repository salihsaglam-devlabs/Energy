using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuote.Commands.UpdateSupplierQuote;

/// <summary>Var olan SupplierQuote kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateSupplierQuoteCommand(Guid Id, UpdateSupplierQuoteRequest Request)
    : IRequest<BaseResponse<bool>>;
