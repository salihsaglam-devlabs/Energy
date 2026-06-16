using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;

namespace Energy.Application.Inventory.StockIssueAllocation.Validators;

/// <summary>UpdateStockIssueAllocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockIssueAllocationRequestValidator : AbstractValidator<UpdateStockIssueAllocationRequest>
{
    public UpdateStockIssueAllocationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.StockDocumentLineId).NotEmpty();
        RuleFor(x => x.StockLotId).NotEmpty();
    }
}
