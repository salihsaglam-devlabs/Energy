using Energy.Shared.Common;
using Energy.Domain.Workflow;
// Düzleştirme sonrası Energy.Application.Workflow.ApprovalRequest bir namespace (entity klasörü)
// olduğundan, domain entity tipini alias ile netleştiriyoruz.
using ApprovalRequestEntity = Energy.Domain.Workflow.ApprovalRequest;

namespace Energy.Application.Workflow.Services;

/// <summary>Bir onay sürecinin nihai sonucu (kaynak belge durumuna yansıtılır).</summary>
public enum ApprovalOutcome
{
    /// <summary>Süreç başlatıldı, onay bekliyor.</summary>
    Pending = 0,
    /// <summary>Tüm gerekli adımlar onaylandı.</summary>
    Approved = 1,
    /// <summary>Reddedildi.</summary>
    Rejected = 2,
    /// <summary>Düzenleme için iade edildi.</summary>
    Returned = 3,
    /// <summary>İptal edildi.</summary>
    Cancelled = 4,
}

/// <summary>Bir onay sürecini başlatmak için gereken bağlam.</summary>
public sealed record StartApprovalRequest(
    string RelatedModule,
    string RelatedEntityType,
    Guid RelatedEntityId,
    Guid RequestedByUserId,
    IReadOnlyDictionary<string, string>? Fields = null);

/// <summary>
/// Dinamik onay akışı motoru. Akış seçimi, snapshot onaycı çözümü, adım aktivasyonu
/// (Sequential / ParallelAny / ParallelAll / Quorum), onay/ret/iade/iptal, delegasyon
/// çözümü, bildirim üretimi ve kaynak belge durum güncellemesini transaction içinde yürütür.
/// </summary>
public interface IApprovalWorkflowService
{
    /// <summary>
    /// Verilen kaynak nesne için yürürlükteki akış versiyonunu seçer, snapshot
    /// onaycılarla bir <see cref="ApprovalRequest"/> başlatır ve ilk adımı aktive eder.
    /// Uygun akış yoksa null döner (onay gerektirmez).
    /// </summary>
    Task<ApprovalRequestEntity?> StartAsync(StartApprovalRequest request, CancellationToken ct = default);

    /// <summary>Geçerli kullanıcının aktif bir adımdaki onayını kaydeder ve akışı ilerletir.</summary>
    Task<ApprovalRequestEntity> ApproveAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default);

    /// <summary>Talebi reddeder; açık adımları kapatır ve kaynak belgeyi reddedilmiş duruma çeker.</summary>
    Task<ApprovalRequestEntity> RejectAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default);

    /// <summary>Talebi düzenleme için iade eder.</summary>
    Task<ApprovalRequestEntity> ReturnAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default);

    /// <summary>Talebi iptal eder (yetkili kullanıcı veya talep sahibi).</summary>
    Task<ApprovalRequestEntity> CancelAsync(Guid approvalRequestId, Guid actingUserId, string? note = null, CancellationToken ct = default);

    /// <summary>Geçerli kullanıcının onay bekleyen taleplerini döndürür.</summary>
    Task<IReadOnlyList<ApprovalRequestEntity>> GetPendingForUserAsync(Guid userId, CancellationToken ct = default);
}

/// <summary>
/// Onay sonucunu kaynak belgeye uygulayan genişletme noktası. Modül-özel durum
/// alanlarını (DocumentStatus / ApprovalRequestStatus / PurchaseOrderStatus ...) doğru
/// şekilde günceller. Workflow motoru bu çağrıyı kendi transaction'ı içinde yapar.
/// </summary>
public interface IApprovalSourceUpdater
{
    /// <summary>Kaynak belgenin durumunu ve onay-talep bağlantısını günceller.</summary>
    Task ApplyAsync(string relatedModule, string relatedEntityType, Guid entityId, Guid approvalRequestId, ApprovalOutcome outcome, CancellationToken ct = default);
}

