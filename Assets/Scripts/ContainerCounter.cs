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

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetParent(parent);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
