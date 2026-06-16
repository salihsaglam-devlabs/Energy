using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;
using Energy.Application.Procurement.Services;
using MediatR;

namespace Energy.Application.Modules.Procurement.Processes.GoodsReceipt.Commands.ExecuteGoodsReceipt;

/// <summary><see cref="ExecuteGoodsReceiptCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ExecuteGoodsReceiptCommandHandler
    : IRequestHandler<ExecuteGoodsReceiptCommand, BaseResponse<bool>>
{
    private readonly IGoodsReceiptService _goodsReceipt;

    public ExecuteGoodsReceiptCommandHandler(IGoodsReceiptService goodsReceipt)
    {
        _goodsReceipt = goodsReceipt;
    }

    public async Task<BaseResponse<bool>> Handle(ExecuteGoodsReceiptCommand request, CancellationToken ct)
    {
        try
        {
            await _goodsReceipt.ReceiveAsync(request.Request.PurchaseReceiptId, ct);
            return BaseResponse<bool>.Success(true, "Completed");
        }
        catch (InvalidOperationException ex)
        {
            return BaseResponse<bool>.Failure(ex.Message);
        }
    }
}
