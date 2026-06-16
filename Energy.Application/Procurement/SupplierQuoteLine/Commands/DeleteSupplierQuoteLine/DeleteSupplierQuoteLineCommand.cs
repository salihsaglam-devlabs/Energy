using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Commands.DeleteSupplierQuoteLine;

/// <summary>SupplierQuoteLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteSupplierQuoteLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
