using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses;
using Energy.Application.Finance.Services;
using MediatR;

namespace Energy.Application.Finance.Processes.PaymentAllocation.Commands.ExecutePaymentAllocation;

/// <summary><see cref="ExecutePaymentAllocationCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ExecutePaymentAllocationCommandHandler
    : IRequestHandler<ExecutePaymentAllocationCommand, BaseResponse<PaymentAllocationProcessResponse>>
{
    private readonly IFinanceService _finance;

    public ExecutePaymentAllocationCommandHandler(IFinanceService finance)
    {
        _finance = finance;
    }

    public async Task<BaseResponse<PaymentAllocationProcessResponse>> Handle(ExecutePaymentAllocationCommand request, CancellationToken ct)
    {
        try
        {
            if (request.Request.Lines is null || request.Request.Lines.Count == 0)
            {
                return BaseResponse<PaymentAllocationProcessResponse>.Failure("At least one allocation line is required.");
            }

            var lines = request.Request.Lines
                .Select(l => new FinanceAllocationLine(l.TargetId, l.Amount))
                .ToList();
            await _finance.AllocatePaymentAsync(request.Request.PaymentId, lines, ct);
            return BaseResponse<PaymentAllocationProcessResponse>.Success(
                new PaymentAllocationProcessResponse
                {
                    AllocatedLineCount = lines.Count,
                    TotalAllocated = lines.Sum(l => l.Amount),
                }, "Completed");
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<PaymentAllocationProcessResponse>.Failure(ex.Message);
        }
    }
}
