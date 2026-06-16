using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Requests;
using Energy.Shared.Models.V1.Finance.Processes.TimesheetCost.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Processes.TimesheetCost.Commands.ExecuteTimesheetCost;

/// <summary>ExecuteTimesheetCost</summary>
public sealed record ExecuteTimesheetCostCommand(TimesheetCostProcessRequest Request)
    : IRequest<BaseResponse<TimesheetCostProcessResponse>>;
