using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.Reports.ProjectStatusReport.Services;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;

namespace Energy.Infrastructure.Modules.Projects.Reports.ProjectStatusReport;

/// <summary>ProjectStatusReport raporu servisi (AsNoTracking, projection, filtre, sayfalama).</summary>
public sealed class ProjectStatusReportService : IProjectStatusReportService
{
    private readonly AppDbContext _db;

    public ProjectStatusReportService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>> GetDataAsync(ProjectStatusReportRequest request, CancellationToken ct = default)
    {
        var query = _db.Projects.AsNoTracking();
        if (request.StartDate.HasValue) query = query.Where(e => e.StartDate >= request.StartDate.Value);
        if (request.EndDate.HasValue) query = query.Where(e => e.StartDate <= request.EndDate.Value);
        var total = await query.CountAsync(ct);
        var pageSize = request.PageSize <= 0 ? 50 : request.PageSize;
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var items = await query
            .OrderByDescending(e => e.StartDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ProjectStatusReportRowResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                ProjectTypeId = e.ProjectTypeId,
                StatusId = e.StatusId,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProjectStatusReportRowResponse>.Create(items, pageNumber, pageSize, total);
        return BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>.Success(page);
    }
}
