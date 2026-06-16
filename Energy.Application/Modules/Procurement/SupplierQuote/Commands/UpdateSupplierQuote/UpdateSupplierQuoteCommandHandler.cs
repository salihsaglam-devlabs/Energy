using Energy.Application.Modules.Procurement.SupplierQuote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Commands.UpdateSupplierQuote;

/// <summary>
/// <see cref="UpdateSupplierQuoteCommand"/> handler'ı. <see cref="ISupplierQuoteService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateSupplierQuoteCommandHandler
    : IRequestHandler<UpdateSupplierQuoteCommand, BaseResponse<bool>>
{
    private readonly ISupplierQuoteService _service;

    public UpdateSupplierQuoteCommandHandler(ISupplierQuoteService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateSupplierQuoteCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
