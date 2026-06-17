namespace Energy.Shared.Models.V1.Core.Company.Responses;

/// <summary>Company lookup öğesi (Id, Code, Name, DisplayName, IsActive standardı).</summary>
public class CompanyLookupResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Kod.</summary>
    public string? Code { get; set; }

    /// <summary>Ad.</summary>
    public string? Name { get; set; }

    /// <summary>Görünen ad.</summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Aktif mi.</summary>
    public bool IsActive { get; set; }
}
