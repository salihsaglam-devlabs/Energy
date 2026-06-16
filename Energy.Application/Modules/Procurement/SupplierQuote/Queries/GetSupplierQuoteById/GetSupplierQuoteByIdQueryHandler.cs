using Energy.Application.Modules.Procurement.SupplierQuote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteById;

/// <summary>
/// <see cref="GetSupplierQuoteByIdQuery"/> handler'ı. <see cref="ISupplierQuoteService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierQuoteByIdQueryHandler
    : IRequestHandler<GetSupplierQuoteByIdQuery, BaseResponse<SupplierQuoteDetailResponse>>
{
    private readonly ISupplierQuoteService _service;

    public GetSupplierQuoteByIdQueryHandler(ISupplierQuoteService service)
        => _service = service;

    public Task<BaseResponse<SupplierQuoteDetailResponse>> Handle(
        GetSupplierQuoteByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
