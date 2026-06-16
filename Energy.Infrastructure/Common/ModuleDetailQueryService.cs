using Energy.Application.Common.Crud;
using Energy.Domain.Common;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Common;

/// <summary>
/// <see cref="IModuleDetailQueryService"/>'nin EF Core uygulaması. Yabancı anahtar
/// alanını <see cref="EF.Property{TProperty}"/> ile dinamik filtreleyerek, bir başlık
/// kaydına bağlı satırları getirir. Global soft-delete sorgu filtresi otomatik uygulanır;
/// sıralama <see cref="AuditableEntity.CreatedAt"/> alanına göre yapılır.
/// </summary>
public sealed class ModuleDetailQueryService : IModuleDetailQueryService
{
    private readonly AppDbContext _db;

    public ModuleDetailQueryService(AppDbContext db) => _db = db;

    public async Task<PaginatedResponse<TChild>> GetChildrenAsync<TChild>(
        string foreignKeyProperty,
        Guid parentId,
        PaginatedRequest request,
        CancellationToken ct = default)
        where TChild : AuditableEntity
    {
        var query = _db.Set<TChild>().AsNoTracking()
            .Where(e => EF.Property<Guid>(e, foreignKeyProperty) == parentId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return PaginatedResponse<TChild>.Create(items, request.PageNumber, request.PageSize, total);
    }
}

