namespace Energy.Domain.Common;

/// <summary>
/// Geriye dönük uyumluluk için <see cref="AuditableEntity"/> takma adı (alias).
/// Eski kodun beklediği <c>BaseEntity</c> ismini korur.
/// </summary>
public abstract class BaseEntity : AuditableEntity
{
}
