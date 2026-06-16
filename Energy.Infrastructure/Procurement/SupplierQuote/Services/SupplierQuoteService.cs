using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.SupplierQuote.Services;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

namespace Energy.Infrastructure.Procurement.SupplierQuote.Services;

/// <summary>SupplierQuote CRUD servisi (projection, pagination, soft-delete).</summary>
public class SupplierQuoteService : ISupplierQuoteService
{
    private readonly AppDbContext _db;

    public SupplierQuoteService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<SupplierQuoteListResponse>>> GetListAsync(GetSupplierQuoteListRequest request, CancellationToken ct = default)
    {
        var query = _db.SupplierQuotes.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new SupplierQuoteListResponse
            {
                Id = e.Id,
                SupplierId = e.SupplierId,
                ProjectId = e.ProjectId,
                CurrencyId = e.CurrencyId,
                QuoteNo = e.QuoteNo,
                QuoteDate = e.QuoteDate,
                PaymentTerm = e.PaymentTerm,
                Status = e.Status,
                IsSelected = e.IsSelected,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<SupplierQuoteListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<SupplierQuoteListResponse>>.Success(page);
    }

    public async Task<BaseResponse<SupplierQuoteDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.SupplierQuotes.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new SupplierQuoteDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                SupplierId = e.SupplierId,
                ProjectId = e.ProjectId,
                CurrencyId = e.CurrencyId,
                QuoteNo = e.QuoteNo,
                QuoteDate = e.QuoteDate,
                PaymentTerm = e.PaymentTerm,
                Status = e.Status,
                IsSelected = e.IsSelected
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<SupplierQuoteDetailResponse>.Failure("NotFound")
            : BaseResponse<SupplierQuoteDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateSupplierQuoteRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Procurement.SupplierQuote
        {
            Id = Guid.NewGuid(),
            SupplierId = request.SupplierId,
            ProjectId = request.ProjectId,
            CurrencyId = request.CurrencyId,
            QuoteNo = request.QuoteNo,
            QuoteDate = request.QuoteDate,
            PaymentTerm = request.PaymentTerm,
            Status = request.Status,
            IsSelected = request.IsSelected,
            CreatedAt = DateTime.UtcNow,
        };
        _db.SupplierQuotes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierQuoteRequest request, CancellationToken ct = default)
    {
        var entity = await _db.SupplierQuotes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SupplierId = request.SupplierId;
            entity.ProjectId = request.ProjectId;
            entity.CurrencyId = request.CurrencyId;
            entity.QuoteNo = request.QuoteNo;
            entity.QuoteDate = request.QuoteDate;
            entity.PaymentTerm = request.PaymentTerm;
            entity.Status = request.Status;
            entity.IsSelected = request.IsSelected;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SupplierQuotes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
