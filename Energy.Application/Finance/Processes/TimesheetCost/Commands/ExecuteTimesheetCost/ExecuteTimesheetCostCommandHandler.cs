using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses;
using Energy.Application.Finance.Services;
using MediatR;

namespace Energy.Application.Finance.Processes.TimesheetCost.Commands.ExecuteTimesheetCost;

/// <summary><see cref="ExecuteTimesheetCostCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ExecuteTimesheetCostCommandHandler
    : IRequestHandler<ExecuteTimesheetCostCommand, BaseResponse<TimesheetCostProcessResponse>>
{
    private readonly IFinanceService _finance;

    public ExecuteTimesheetCostCommandHandler(IFinanceService finance)
    {
        _finance = finance;
    }

    public async Task<BaseResponse<TimesheetCostProcessResponse>> Handle(ExecuteTimesheetCostCommand request, CancellationToken ct)
    {
        try
        {
            var id = await _finance.PostTimesheetCostAsync(request.Request.TimesheetId, request.Request.CurrencyId, ct);
            return BaseResponse<TimesheetCostProcessResponse>.Success(
                new TimesheetCostProcessResponse { FinancialTransactionId = id }, "Completed");
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<TimesheetCostProcessResponse>.Failure(ex.Message);
        }
    }
}
