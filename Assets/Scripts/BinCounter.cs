using System;
using UnityEngine;

public class BinCounter : BaseCounter
{
    public static event EventHandler OnTrash;

    public override void Interact(IKitchenObjectParent parent)
    {
        OnTrash?.Invoke(this, EventArgs.Empty);
        parent.GetKitchenObject().DestroySelf();
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
