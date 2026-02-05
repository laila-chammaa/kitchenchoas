using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField]
    protected KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(IKitchenObjectParent parent)
    {
        if (parent.HasKitchenObject())
            return;

        KitchenObject.SpawnKitchenObject(kitchenObjectSO, parent);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
