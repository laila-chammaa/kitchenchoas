using UnityEngine;

public class BinCounter : BaseCounter
{
    public override void Interact(IKitchenObjectParent parent)
    {
        parent.GetKitchenObject().DestroySelf();
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
