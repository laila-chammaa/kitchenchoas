using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(IKitchenObjectParent parent)
    {
        if (parent.HasKitchenObject() && parent.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
        {
            plateKitchenObject.DestroySelf();
        }
    }
    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
