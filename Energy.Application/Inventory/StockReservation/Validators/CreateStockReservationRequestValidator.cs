using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;

namespace Energy.Application.Inventory.StockReservation.Validators;

/// <summary>CreateStockReservationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockReservationRequestValidator : AbstractValidator<CreateStockReservationRequest>
{
    public CreateStockReservationRequestValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
