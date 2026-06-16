using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Processes.ProgressPaymentPosting.Commands.ExecuteProgressPaymentPosting;

/// <summary>ExecuteProgressPaymentPosting</summary>
public sealed record ExecuteProgressPaymentPostingCommand(ProgressPaymentPostingProcessRequest Request)
    : IRequest<BaseResponse<ProgressPaymentPostingProcessResponse>>;
