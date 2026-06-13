using Energy.Domain.Common;

namespace Energy.Domain.Documents;

/// <summary>Belge klasörü (hiyerarşik).</summary>
public class DocumentFolder : AuditableEntity
{
    public Guid? ParentFolderId { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>Belge kaydı. Tüm modüllere bağlanabilir ama hiçbirine zorunlu bağımlı değildir.</summary>
public class Document : AuditableEntity
{
    public Guid? DocumentFolderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public int CurrentVersionNo { get; set; }
}

/// <summary>Belge versiyonu (fiziksel dosya tek kez saklanır, yeni yükleme yeni versiyondur).</summary>
public class DocumentVersion : AuditableEntity
{
    public Guid DocumentId { get; set; }
    public int VersionNo { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? ContentType { get; set; }
    public DateTime UploadedAt { get; set; }
}

/// <summary>Belge ↔ iş nesnesi bağlantısı (çok biçimli).</summary>
public class DocumentRelation : AuditableEntity
{
    public Guid DocumentId { get; set; }
    public string RelatedModule { get; set; } = string.Empty;
    public string RelatedEntityType { get; set; } = string.Empty;
    public Guid RelatedEntityId { get; set; }
}

/// <summary>Belge erişim yetkisi (kullanıcı veya rol bazlı).</summary>
public class DocumentPermission : AuditableEntity
{
    public Guid DocumentId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
    /// <summary>Read, Write, Manage.</summary>
    public string AccessType { get; set; } = "Read";
}

