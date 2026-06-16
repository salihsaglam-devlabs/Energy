using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Processes.PaymentAllocation.Commands.ExecutePaymentAllocation;

/// <summary>ExecutePaymentAllocation</summary>
public sealed record ExecutePaymentAllocationCommand(PaymentAllocationProcessRequest Request)
    : IRequest<BaseResponse<PaymentAllocationProcessResponse>>;
