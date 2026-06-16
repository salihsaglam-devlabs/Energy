using Energy.Domain.Common;

namespace Energy.Domain.Core;

/// <summary>Ana organizasyon kökü (şirket).</summary>
public class Company : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Ana para birimi. <see cref="Currency"/> FK.</summary>
    public Guid BaseCurrencyId { get; set; }
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Şube. <see cref="Company"/>'ye bağlıdır.</summary>
public class Branch : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Departman. Şirkete bağlı ve hiyerarşik olabilir.</summary>
public class Department : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid? ParentDepartmentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Departman yöneticisi (opsiyonel) — DepartmentManager onaycı tipi için kullanılır.</summary>
    public Guid? ManagerUserId { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Para birimi.</summary>
public class Currency : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Kur kaydı. Belirli bir tarihte bir para biriminin ana para birimine oranı.</summary>
public class ExchangeRate : AuditableEntity
{
    public Guid CurrencyId { get; set; }
    public DateTime RateDate { get; set; }
    public decimal Rate { get; set; }
}

/// <summary>Ölçü birimi.</summary>
public class UnitOfMeasure : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>Genel birim dönüşümü (malzemeden bağımsız).</summary>
public class UnitConversion : AuditableEntity
{
    public Guid FromUnitOfMeasureId { get; set; }
    public Guid ToUnitOfMeasureId { get; set; }
    public decimal Factor { get; set; }
}

/// <summary>Belge numarası üretim tanımı (modül bazlı).</summary>
public class SequenceDefinition : AuditableEntity
{
    public string Module { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public int Padding { get; set; } = 6;
    public long NextNumber { get; set; } = 1;
    public string? Format { get; set; }
}

/// <summary>Sistem genel ayarı (anahtar/değer).</summary>
public class SystemSetting : AuditableEntity
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Category { get; set; }
    public string? DescriptionKey { get; set; }
}

