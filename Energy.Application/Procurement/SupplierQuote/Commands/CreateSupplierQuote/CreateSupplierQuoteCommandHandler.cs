using Energy.Application.Procurement.SupplierQuote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuote.Commands.CreateSupplierQuote;

/// <summary>
/// <see cref="CreateSupplierQuoteCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ISupplierQuoteService"/>'i orkestre eder.
/// </summary>
public sealed class CreateSupplierQuoteCommandHandler
    : IRequestHandler<CreateSupplierQuoteCommand, BaseResponse<Guid>>
{
    private readonly ISupplierQuoteService _service;

    public CreateSupplierQuoteCommandHandler(ISupplierQuoteService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateSupplierQuoteCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
