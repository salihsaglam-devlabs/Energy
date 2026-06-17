namespace Energy.Shared.Models.V1.Inventory.StockReservation.Requests;

/// <summary>StockReservation oluşturma isteği.</summary>
public class CreateStockReservationRequest
{
    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>RelatedModule</summary>
    public string? RelatedModule { get; set; }

    /// <summary>RelatedEntityType</summary>
    public string? RelatedEntityType { get; set; }

    /// <summary>RelatedEntityId</summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>IsReleased</summary>
    public bool IsReleased { get; set; }
}
