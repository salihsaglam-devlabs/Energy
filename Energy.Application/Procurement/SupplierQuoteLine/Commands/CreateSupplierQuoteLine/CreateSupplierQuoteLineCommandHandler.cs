using Energy.Application.Procurement.SupplierQuoteLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Commands.CreateSupplierQuoteLine;

/// <summary>
/// <see cref="CreateSupplierQuoteLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ISupplierQuoteLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateSupplierQuoteLineCommandHandler
    : IRequestHandler<CreateSupplierQuoteLineCommand, BaseResponse<Guid>>
{
    private readonly ISupplierQuoteLineService _service;

    public CreateSupplierQuoteLineCommandHandler(ISupplierQuoteLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateSupplierQuoteLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
