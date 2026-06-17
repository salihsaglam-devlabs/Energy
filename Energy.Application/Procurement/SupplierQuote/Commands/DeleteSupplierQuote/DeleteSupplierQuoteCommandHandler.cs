using Energy.Application.Procurement.SupplierQuote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuote.Commands.DeleteSupplierQuote;

/// <summary>
/// <see cref="DeleteSupplierQuoteCommand"/> handler'ı. <see cref="ISupplierQuoteService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteSupplierQuoteCommandHandler
    : IRequestHandler<DeleteSupplierQuoteCommand, BaseResponse<bool>>
{
    private readonly ISupplierQuoteService _service;

    public DeleteSupplierQuoteCommandHandler(ISupplierQuoteService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteSupplierQuoteCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
