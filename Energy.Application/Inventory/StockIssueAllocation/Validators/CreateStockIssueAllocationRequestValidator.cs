using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;

namespace Energy.Application.Inventory.StockIssueAllocation.Validators;

/// <summary>CreateStockIssueAllocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockIssueAllocationRequestValidator : AbstractValidator<CreateStockIssueAllocationRequest>
{
    public CreateStockIssueAllocationRequestValidator()
    {
        RuleFor(x => x.StockDocumentLineId).NotEmpty();
        RuleFor(x => x.StockLotId).NotEmpty();
    }
}
