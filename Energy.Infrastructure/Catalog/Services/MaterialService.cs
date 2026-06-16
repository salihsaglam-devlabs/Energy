using Energy.Application.Catalog.Services;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Catalog.Services;

/// <summary>
/// <see cref="IMaterialService"/> uygulaması. Kategori bazlı dinamik öznitelik
/// doğrulaması, aktive engeli ve baz birim değişiklik kısıtı.
/// </summary>
public sealed class MaterialService : IMaterialService
{
    private readonly AppDbContext _db;
    private readonly ILogger<MaterialService> _logger;

    public MaterialService(AppDbContext db, ILogger<MaterialService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IReadOnlyList<string>> ValidateAttributesAsync(Guid materialId, CancellationToken ct = default)
    {
        var material = await _db.Materials.FirstOrDefaultAsync(m => m.Id == materialId, ct)
            ?? throw new InvalidOperationException($"Material {materialId} not found.");

        // Kategorinin öznitelik bağlantıları + tanımları.
        var categoryAttributes = await (
            from link in _db.MaterialCategoryAttributes
            join def in _db.MaterialAttributeDefinitions on link.MaterialAttributeDefinitionId equals def.Id
            where link.MaterialCategoryId == material.MaterialCategoryId
            select new { def.Id, def.Code, def.DataType, link.IsRequired })
            .ToListAsync(ct);

        var values = await _db.MaterialAttributeValues
            .Where(v => v.MaterialId == materialId)
            .ToListAsync(ct);

        var errors = new List<string>();
        foreach (var attribute in categoryAttributes)
        {
            var value = values.FirstOrDefault(v => v.MaterialAttributeDefinitionId == attribute.Id);

            var hasValue = value is not null && attribute.DataType switch
            {
                "Number" or "Decimal" => value.ValueNumber is not null,
                "Boolean" => value.ValueBoolean is not null,
                "Date" => value.ValueDate is not null,
                "Option" => value.OptionId is not null,
                _ => !string.IsNullOrWhiteSpace(value.ValueText),
            };

            if (attribute.IsRequired && !hasValue)
            {
                errors.Add($"Required attribute '{attribute.Code}' is missing or invalid.");
                continue;
            }

            // Option değeri varsa, tanıma ait geçerli bir seçenek olmalı.
            if (value?.OptionId is { } optionId)
            {
                var validOption = await _db.MaterialAttributeOptions
                    .AnyAsync(o => o.Id == optionId && o.MaterialAttributeDefinitionId == attribute.Id, ct);
                if (!validOption)
                {
                    errors.Add($"Attribute '{attribute.Code}' references an invalid option.");
                }
            }
        }

        return errors;
    }

    public async Task ActivateAsync(Guid materialId, CancellationToken ct = default)
    {
        var errors = await ValidateAttributesAsync(materialId, ct);
        if (errors.Count > 0)
        {
            throw new InvalidOperationException(
                "Cannot activate material; attribute validation failed: " + string.Join(" ", errors));
        }

        var material = await _db.Materials.FirstAsync(m => m.Id == materialId, ct);
        material.IsActive = true;
        await _db.SaveChangesAsync(ct);
    }

    public async Task ChangeBaseUnitOfMeasureAsync(Guid materialId, Guid newUnitOfMeasureId, CancellationToken ct = default)
    {
        var material = await _db.Materials.FirstOrDefaultAsync(m => m.Id == materialId, ct)
            ?? throw new InvalidOperationException($"Material {materialId} not found.");

        var hasMovements = await _db.StockTransactions.AnyAsync(t => t.MaterialId == materialId, ct);
        if (hasMovements)
        {
            throw new InvalidOperationException(
                "Cannot change base unit of measure: the material already has stock movements.");
        }

        material.BaseUnitOfMeasureId = newUnitOfMeasureId;
        await _db.SaveChangesAsync(ct);
    }
}

