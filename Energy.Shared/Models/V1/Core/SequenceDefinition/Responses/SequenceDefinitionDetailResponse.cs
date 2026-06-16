namespace Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

/// <summary>SequenceDefinition detay görünümü.</summary>
public class SequenceDefinitionDetailResponse
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

    /// <summary>Module</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>EntityType</summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>Prefix</summary>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>Padding</summary>
    public int Padding { get; set; }

    /// <summary>NextNumber</summary>
    public long NextNumber { get; set; }

    /// <summary>Format</summary>
    public string? Format { get; set; }
}
