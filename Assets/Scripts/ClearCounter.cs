using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject())
        {
            // If parent has object, drop it here
            if (parent.HasKitchenObject())
            {
                var droppedObject = parent.GetKitchenObject();
                droppedObject.SetParent(this);
            }
        }
        // If the player is holding a plate
        else if (parent.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();
            }
        }
        // If there is a plate on the counter
        else if (GetKitchenObject() is PlateKitchenObject plate && parent.HasKitchenObject())
        {
            if (plate.TryAddIngredient(parent.GetKitchenObject().GetKitchenObjectSO()))
            {
                parent.GetKitchenObject().DestroySelf();
            }
        }
        else if (!parent.HasKitchenObject())
        {
            GetKitchenObject().SetParent(parent);
        }
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
