using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;

namespace Energy.Application.Inventory.StockLot.Validators;

/// <summary>CreateStockLotRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockLotRequestValidator : AbstractValidator<CreateStockLotRequest>
{
    public CreateStockLotRequestValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.SourceStockDocumentLineId).NotEmpty();
        RuleFor(x => x.LotNo).NotEmpty();
    }
}
