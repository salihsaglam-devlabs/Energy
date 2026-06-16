using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuote.Commands.DeleteSupplierQuote;

/// <summary>SupplierQuote kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteSupplierQuoteCommand(Guid Id) : IRequest<BaseResponse<bool>>;
