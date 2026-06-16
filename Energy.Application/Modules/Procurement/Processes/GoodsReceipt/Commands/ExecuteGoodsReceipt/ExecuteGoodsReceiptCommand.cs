using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;
using MediatR;

namespace Energy.Application.Modules.Procurement.Processes.GoodsReceipt.Commands.ExecuteGoodsReceipt;

/// <summary>ExecuteGoodsReceipt</summary>
public sealed record ExecuteGoodsReceiptCommand(GoodsReceiptProcessRequest Request)
    : IRequest<BaseResponse<bool>>;
