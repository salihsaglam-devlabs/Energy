using Energy.Application.Procurement.SupplierQuoteLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Commands.DeleteSupplierQuoteLine;

/// <summary>
/// <see cref="DeleteSupplierQuoteLineCommand"/> handler'ı. <see cref="ISupplierQuoteLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteSupplierQuoteLineCommandHandler
    : IRequestHandler<DeleteSupplierQuoteLineCommand, BaseResponse<bool>>
{
    private readonly ISupplierQuoteLineService _service;

    public DeleteSupplierQuoteLineCommandHandler(ISupplierQuoteLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteSupplierQuoteLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
