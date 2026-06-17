using Energy.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.Contract.Services;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;

namespace Energy.Infrastructure.Contracts.Contract.Services;

/// <summary>Contract CRUD servisi (projection, pagination, soft-delete).</summary>
public class ContractService : IContractService
{
    private readonly AppDbContext _db;

    public ContractService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ContractListResponse>>> GetListAsync(GetContractListRequest request, CancellationToken ct = default)
    {
        var query = _db.Contracts.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ContractListResponse
            {
                Id = e.Id,
                ContractType = e.ContractType,
                ProjectId = e.ProjectId,
                ContractNo = e.ContractNo,
                CurrencyId = e.CurrencyId,
                ContractAmount = e.ContractAmount,
                Title = e.Title,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ContractListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ContractListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ContractDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Contracts.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ContractDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ContractType = e.ContractType,
                ProjectId = e.ProjectId,
                ContractNo = e.ContractNo,
                CurrencyId = e.CurrencyId,
                ContractAmount = e.ContractAmount,
                Title = e.Title,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Status = e.Status
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ContractDetailResponse>.Failure("NotFound")
            : BaseResponse<ContractDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateContractRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Contracts.Contract
        {
            Id = Guid.NewGuid(),
            ContractType = request.ContractType,
            ProjectId = request.ProjectId,
            ContractNo = request.ContractNo,
            CurrencyId = request.CurrencyId,
            ContractAmount = request.ContractAmount,
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Contracts.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Contracts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ContractType = request.ContractType;
            entity.ProjectId = request.ProjectId;
            entity.ContractNo = request.ContractNo;
            entity.CurrencyId = request.CurrencyId;
            entity.ContractAmount = request.ContractAmount;
            entity.Title = request.Title;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Contracts.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
