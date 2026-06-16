using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.SequenceDefinition.Services;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Requests;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

namespace Energy.Infrastructure.Modules.Core.SequenceDefinition.Services;

/// <summary>SequenceDefinition CRUD servisi (projection, pagination, soft-delete).</summary>
public class SequenceDefinitionService : ISequenceDefinitionService
{
    private readonly EnergyDbContext _db;

    public SequenceDefinitionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>> GetListAsync(GetSequenceDefinitionListRequest request, CancellationToken ct = default)
    {
        var query = _db.SequenceDefinitions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new SequenceDefinitionListResponse
            {
                Id = e.Id,
                Module = e.Module,
                EntityType = e.EntityType,
                Prefix = e.Prefix,
                Padding = e.Padding,
                NextNumber = e.NextNumber,
                Format = e.Format,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<SequenceDefinitionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<SequenceDefinitionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<SequenceDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.SequenceDefinitions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new SequenceDefinitionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Module = e.Module,
                EntityType = e.EntityType,
                Prefix = e.Prefix,
                Padding = e.Padding,
                NextNumber = e.NextNumber,
                Format = e.Format
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<SequenceDefinitionDetailResponse>.Failure("NotFound")
            : BaseResponse<SequenceDefinitionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateSequenceDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.SequenceDefinition
        {
            Id = Guid.NewGuid(),
            Module = request.Module,
            EntityType = request.EntityType,
            Prefix = request.Prefix,
            Padding = request.Padding,
            NextNumber = request.NextNumber,
            Format = request.Format,
            CreatedAt = DateTime.UtcNow,
        };
        _db.SequenceDefinitions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSequenceDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.SequenceDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Module = request.Module;
            entity.EntityType = request.EntityType;
            entity.Prefix = request.Prefix;
            entity.Padding = request.Padding;
            entity.NextNumber = request.NextNumber;
            entity.Format = request.Format;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SequenceDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
