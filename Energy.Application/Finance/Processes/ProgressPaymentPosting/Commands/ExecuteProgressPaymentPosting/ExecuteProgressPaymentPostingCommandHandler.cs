using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses;
using Energy.Application.Finance.Services;
using MediatR;

namespace Energy.Application.Finance.Processes.ProgressPaymentPosting.Commands.ExecuteProgressPaymentPosting;

/// <summary><see cref="ExecuteProgressPaymentPostingCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ExecuteProgressPaymentPostingCommandHandler
    : IRequestHandler<ExecuteProgressPaymentPostingCommand, BaseResponse<ProgressPaymentPostingProcessResponse>>
{
    private readonly IFinanceService _finance;

    public ExecuteProgressPaymentPostingCommandHandler(IFinanceService finance)
    {
        _finance = finance;
    }

    public async Task<BaseResponse<ProgressPaymentPostingProcessResponse>> Handle(ExecuteProgressPaymentPostingCommand request, CancellationToken ct)
    {
        try
        {
            var id = await _finance.PostProgressPaymentAsync(request.Request.ProgressPaymentId, ct);
            return BaseResponse<ProgressPaymentPostingProcessResponse>.Success(
                new ProgressPaymentPostingProcessResponse { FinancialTransactionId = id }, "Completed");
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<ProgressPaymentPostingProcessResponse>.Failure(ex.Message);
        }
    }
}
