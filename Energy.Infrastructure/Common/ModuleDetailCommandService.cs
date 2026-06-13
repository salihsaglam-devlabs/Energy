using Energy.Application.Common.Crud;
using Energy.Domain.Common;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Common;

/// <summary>
/// <see cref="IModuleDetailCommandService"/>'nin EF Core uygulaması. Alt satırların
/// başlık bağını (yabancı anahtarı) yetkili biçimde yöneterek, audit ve soft-delete
/// farkında oluşturma/güncelleme/silme sağlar. Audit alanları <c>AuditingSaveChangesInterceptor</c>
/// tarafından damgalanır; silme interceptor tarafından yumuşak silmeye dönüştürülür.
/// </summary>
public sealed class ModuleDetailCommandService : IModuleDetailCommandService
{
    private readonly AppDbContext _db;

    public ModuleDetailCommandService(AppDbContext db) => _db = db;

    public async Task<TChild> CreateChildAsync<TChild>(
        string foreignKeyProperty, Guid parentId, TChild entity, CancellationToken ct = default)
        where TChild : AuditableEntity
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = null;

        _db.Set<TChild>().Add(entity);
        // Başlık bağını her durumda zorla — gövdeden gelen yabancı anahtar değeri yok sayılır.
        _db.Entry(entity).Property(foreignKeyProperty).CurrentValue = parentId;

        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<TChild?> UpdateChildAsync<TChild>(
        string foreignKeyProperty, Guid id, TChild entity, CancellationToken ct = default)
        where TChild : AuditableEntity
    {
        var existing = await _db.Set<TChild>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (existing is null)
        {
            return null;
        }

        // Oluşturma denetimini, soft-delete durumunu ve başlık bağını koru.
        var createdAt = existing.CreatedAt;
        var createdBy = existing.CreatedBy;
        var parentId = _db.Entry(existing).Property(foreignKeyProperty).CurrentValue;

        entity.Id = id;
        _db.Entry(existing).CurrentValues.SetValues(entity);

        existing.CreatedAt = createdAt;
        existing.CreatedBy = createdBy;
        existing.IsDeleted = false;
        existing.DeletedAt = null;
        existing.DeletedBy = null;
        // Güncelleme yoluyla başka bir başlığa taşımayı engelle.
        _db.Entry(existing).Property(foreignKeyProperty).CurrentValue = parentId;

        await _db.SaveChangesAsync(ct);
        return existing;
    }

    public async Task<bool> DeleteChildAsync<TChild>(Guid id, CancellationToken ct = default)
        where TChild : AuditableEntity
    {
        var existing = await _db.Set<TChild>().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (existing is null)
        {
            return false;
        }

        _db.Set<TChild>().Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

