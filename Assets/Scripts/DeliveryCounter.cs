using System;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (parent.HasKitchenObject() && parent.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
        {
            DeliverManager.Instance.DeliverRecipe(plateKitchenObject);
            plateKitchenObject.DestroySelf();
        }
    }
    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
