using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.Employee.Services;
using Energy.Shared.Models.V1.Organization.Employee.Requests;
using Energy.Shared.Models.V1.Organization.Employee.Responses;

namespace Energy.Infrastructure.Modules.Organization.Employee.Services;

/// <summary>Employee CRUD servisi (projection, pagination, soft-delete).</summary>
public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _db;

    public EmployeeService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<EmployeeListResponse>>> GetListAsync(GetEmployeeListRequest request, CancellationToken ct = default)
    {
        var query = _db.Employees.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EmployeeListResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                BranchId = e.BranchId,
                DepartmentId = e.DepartmentId,
                EmployeePositionId = e.EmployeePositionId,
                UserId = e.UserId,
                Code = e.Code,
                FirstName = e.FirstName,
                LastName = e.LastName,
                NationalId = e.NationalId,
                Phone = e.Phone,
                Email = e.Email,
                HireDate = e.HireDate,
                TerminationDate = e.TerminationDate,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<EmployeeListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<EmployeeListResponse>>.Success(page);
    }

    public async Task<BaseResponse<EmployeeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Employees.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new EmployeeDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                CompanyId = e.CompanyId,
                BranchId = e.BranchId,
                DepartmentId = e.DepartmentId,
                EmployeePositionId = e.EmployeePositionId,
                UserId = e.UserId,
                Code = e.Code,
                FirstName = e.FirstName,
                LastName = e.LastName,
                NationalId = e.NationalId,
                Phone = e.Phone,
                Email = e.Email,
                HireDate = e.HireDate,
                TerminationDate = e.TerminationDate,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<EmployeeDetailResponse>.Failure("NotFound")
            : BaseResponse<EmployeeDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Organization.Employee
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            BranchId = request.BranchId,
            DepartmentId = request.DepartmentId,
            EmployeePositionId = request.EmployeePositionId,
            UserId = request.UserId,
            Code = request.Code,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NationalId = request.NationalId,
            Phone = request.Phone,
            Email = request.Email,
            HireDate = request.HireDate,
            TerminationDate = request.TerminationDate,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Employees.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CompanyId = request.CompanyId;
            entity.BranchId = request.BranchId;
            entity.DepartmentId = request.DepartmentId;
            entity.EmployeePositionId = request.EmployeePositionId;
            entity.UserId = request.UserId;
            entity.Code = request.Code;
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.NationalId = request.NationalId;
            entity.Phone = request.Phone;
            entity.Email = request.Email;
            entity.HireDate = request.HireDate;
            entity.TerminationDate = request.TerminationDate;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
