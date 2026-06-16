namespace Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

/// <summary>ExpenseClaimLine lookup öğesi (Id, Code, Name, DisplayName, IsActive standardı).</summary>
public class ExpenseClaimLineLookupResponse
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
