namespace Energy.Shared.Models.V1.Workflow.Processes.Approval.Responses;

/// <summary>Onay gelen kutusu satırı (salt-okunur, domain sızdırmaz).</summary>
public sealed class ApprovalRequestListItemResponse
{
    /// <summary>Onay talebi kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Kaynak modül adı.</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>Kaynak nesne türü.</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>Kaynak nesne kimliği.</summary>
    public Guid RelatedEntityId { get; set; }

    /// <summary>Talebin güncel durumu (metin).</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Aktif adım numarası.</summary>
    public int CurrentStepNo { get; set; }

    /// <summary>Talep oluşturulma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
