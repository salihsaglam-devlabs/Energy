using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuoteLine.Commands.CreateSupplierQuoteLine;

/// <summary>Yeni SupplierQuoteLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateSupplierQuoteLineCommand(CreateSupplierQuoteLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
