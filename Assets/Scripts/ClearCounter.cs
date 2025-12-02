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
        else if (!parent.HasKitchenObject())
        {
            GetKitchenObject().SetParent(parent);
        }
    }
}
