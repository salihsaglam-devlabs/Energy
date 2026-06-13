using Energy.Application.Common.Crud;
using Energy.Domain.Common;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Common;

/// <summary>
/// <see cref="IGenericCrudService{TEntity}"/>'nin EF Core uygulaması. Tüm kurumsal
/// modül varlıkları için ortak, audit ve soft-delete farkında CRUD davranışı sağlar.
/// Audit alanları <c>AuditingSaveChangesInterceptor</c> tarafından damgalanır; silme
/// işlemi interceptor tarafından yumuşak silmeye dönüştürülür.
/// </summary>
public sealed class GenericCrudService<TEntity> : IGenericCrudService<TEntity>
    where TEntity : AuditableEntity
{
    private readonly AppDbContext _db;

    public GenericCrudService(AppDbContext db) => _db = db;

    public async Task<PaginatedResponse<TEntity>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
    {
        var query = _db.Set<TEntity>().AsNoTracking();

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return PaginatedResponse<TEntity>.Create(items, request.PageNumber, request.PageSize, total);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        // Audit / soft-delete alanları interceptor tarafından yönetilir.
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;

        _db.Set<TEntity>().Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<TEntity?> UpdateAsync(Guid id, TEntity entity, CancellationToken ct = default)
    {
        var existing = await _db.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (existing is null)
        {
            return null;
        }

        // Oluşturma denetimini ve soft-delete durumunu koru; yalnızca iş alanlarını güncelle.
        var createdAt = existing.CreatedAt;
        var createdBy = existing.CreatedBy;

        entity.Id = id;
        _db.Entry(existing).CurrentValues.SetValues(entity);

        existing.CreatedAt = createdAt;
        existing.CreatedBy = createdBy;
        existing.IsDeleted = false;
        existing.DeletedAt = null;
        existing.DeletedBy = null;

        await _db.SaveChangesAsync(ct);
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var existing = await _db.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (existing is null)
        {
            return false;
        }

        // Interceptor bu Remove çağrısını yumuşak silmeye dönüştürür.
        _db.Set<TEntity>().Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

