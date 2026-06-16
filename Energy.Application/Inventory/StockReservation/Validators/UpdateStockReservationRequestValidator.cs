using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;

namespace Energy.Application.Inventory.StockReservation.Validators;

/// <summary>UpdateStockReservationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockReservationRequestValidator : AbstractValidator<UpdateStockReservationRequest>
{
    public UpdateStockReservationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
