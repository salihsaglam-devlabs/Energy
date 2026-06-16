namespace Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;

/// <summary>WorkOrderMaterialPlan oluşturma isteği.</summary>
public class CreateWorkOrderMaterialPlanRequest
{
    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>PlannedQuantity</summary>
    public decimal PlannedQuantity { get; set; }
}
