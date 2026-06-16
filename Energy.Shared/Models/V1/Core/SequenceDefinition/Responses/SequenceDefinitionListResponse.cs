namespace Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

/// <summary>SequenceDefinition liste satırı.</summary>
public class SequenceDefinitionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
