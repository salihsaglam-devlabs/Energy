using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Requests;
using Energy.Shared.Models.V1.Organization.Employee.Responses;

namespace Energy.Application.Organization.Employee.Services;

/// <summary>Employee CRUD use-case sözleşmesi.</summary>
public interface IEmployeeService
{
    /// <summary>Sayfalanmış Employee listesi.</summary>
    Task<BaseResponse<PaginatedResponse<EmployeeListResponse>>> GetListAsync(GetEmployeeListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<EmployeeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
