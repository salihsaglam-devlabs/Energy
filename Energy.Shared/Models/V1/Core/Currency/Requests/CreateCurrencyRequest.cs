namespace Energy.Shared.Models.V1.Core.Currency.Requests;

/// <summary>Currency oluşturma isteği.</summary>
public class CreateCurrencyRequest
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Symbol</summary>
    public string? Symbol { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
