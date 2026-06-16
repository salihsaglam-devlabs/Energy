using Energy.Application.Procurement.SupplierQuoteLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Commands.UpdateSupplierQuoteLine;

/// <summary>
/// <see cref="UpdateSupplierQuoteLineCommand"/> handler'ı. <see cref="ISupplierQuoteLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateSupplierQuoteLineCommandHandler
    : IRequestHandler<UpdateSupplierQuoteLineCommand, BaseResponse<bool>>
{
    private readonly ISupplierQuoteLineService _service;

    public UpdateSupplierQuoteLineCommandHandler(ISupplierQuoteLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateSupplierQuoteLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
