using Energy.Application.Modules.Procurement.SupplierQuote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteList;

/// <summary>
/// <see cref="GetSupplierQuoteListQuery"/> handler'ı. <see cref="ISupplierQuoteService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierQuoteListQueryHandler
    : IRequestHandler<GetSupplierQuoteListQuery, BaseResponse<PaginatedResponse<SupplierQuoteListResponse>>>
{
    private readonly ISupplierQuoteService _service;

    public GetSupplierQuoteListQueryHandler(ISupplierQuoteService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<SupplierQuoteListResponse>>> Handle(
        GetSupplierQuoteListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
