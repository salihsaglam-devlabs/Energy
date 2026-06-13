using Energy.Shared.Models.V1.Home.Requests;
using Energy.Shared.Models.V1.Home.Responses;

namespace Energy.Application.Home.Services;

/// <summary>Ana sayfa/gösterge paneli (dashboard) verilerini sağlayan servis.</summary>
public interface IHomeService
{
    /// <summary>İstenen parametrelere göre gösterge paneli özet verisini döndürür.</summary>
    Task<HomeDashboardResponse> GetDashboardAsync(GetHomeDashboardRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kurumsal (iş) modül widget'larının canlı metriklerini döndürür. Yalnızca çağıran
    /// kullanıcının yetkili olduğu (widget'ın gerektirdiği yetki) ve etkin widget'lar dahil edilir.
    /// </summary>
    Task<IReadOnlyList<EnterpriseMetricResponse>> GetEnterpriseMetricsAsync(CancellationToken cancellationToken = default);
}
