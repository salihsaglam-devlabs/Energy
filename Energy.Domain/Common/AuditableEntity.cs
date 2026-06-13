namespace Energy.Domain.Common;

/// <summary>
/// Tüm aggregate root'ların paylaştığı ortak denetim (audit) ve yumuşak silme
/// (soft-delete) sözleşmesi. Join (bağlantı) tabloları ve yalnızca-ekleme yapılan
/// log kayıtları bu tipten türemez.
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>Birincil anahtar (primary key).</summary>
    public Guid Id { get; set; }

    /// <summary>Kaydın oluşturulduğu UTC zaman damgası.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Kaydı oluşturan kullanıcının kimliği (varsa).</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Kaydın en son güncellendiği UTC zaman damgası (hiç güncellenmediyse null).</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Kaydı en son güncelleyen kullanıcının kimliği (varsa).</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Yumuşak silme işareti: true ise kayıt mantıksal olarak silinmiştir.</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Kaydın yumuşak silindiği UTC zaman damgası (varsa).</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Kaydı yumuşak silen kullanıcının kimliği (varsa).</summary>
    public Guid? DeletedBy { get; set; }
}
