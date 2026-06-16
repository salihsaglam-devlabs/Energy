using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Commands.UpdateSupplierQuoteLine;

/// <summary>Var olan SupplierQuoteLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateSupplierQuoteLineCommand(Guid Id, UpdateSupplierQuoteLineRequest Request)
    : IRequest<BaseResponse<bool>>;
