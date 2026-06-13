using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;

namespace Energy.Application.Home.Services;

/// <summary>Ana sayfa/gösterge paneli (dashboard) verilerini sağlayan servis.</summary>
public interface IHomeService
{
    /// <summary>İstenen parametrelere göre gösterge paneli özet verisini döndürür.</summary>
    Task<HomeDashboardResponse> GetDashboardAsync(GetHomeDashboardRequest request, CancellationToken cancellationToken = default);
}
