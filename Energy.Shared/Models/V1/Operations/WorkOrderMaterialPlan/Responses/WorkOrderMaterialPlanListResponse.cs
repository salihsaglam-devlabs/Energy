namespace Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;

/// <summary>WorkOrderMaterialPlan liste satırı.</summary>
public class WorkOrderMaterialPlanListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>PlannedQuantity</summary>
    public decimal PlannedQuantity { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
