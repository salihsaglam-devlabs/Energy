using Energy.Application.Procurement.SupplierQuoteLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineById;

/// <summary>
/// <see cref="GetSupplierQuoteLineByIdQuery"/> handler'ı. <see cref="ISupplierQuoteLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierQuoteLineByIdQueryHandler
    : IRequestHandler<GetSupplierQuoteLineByIdQuery, BaseResponse<SupplierQuoteLineDetailResponse>>
{
    private readonly ISupplierQuoteLineService _service;

    public GetSupplierQuoteLineByIdQueryHandler(ISupplierQuoteLineService service)
        => _service = service;

    public Task<BaseResponse<SupplierQuoteLineDetailResponse>> Handle(
        GetSupplierQuoteLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
