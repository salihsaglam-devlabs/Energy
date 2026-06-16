namespace Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;

/// <summary>ContractAmendment detay görünümü.</summary>
public class ContractAmendmentDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>ContractId</summary>
    public Guid ContractId { get; set; }

    /// <summary>AmendmentNo</summary>
    public string AmendmentNo { get; set; } = string.Empty;

    /// <summary>AmendmentDate</summary>
    public DateTime AmendmentDate { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>AmountDelta</summary>
    public decimal AmountDelta { get; set; }
}
