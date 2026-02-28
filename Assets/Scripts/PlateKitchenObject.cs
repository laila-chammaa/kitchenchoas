using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    const float k_IngredientHeight = 0.1f;

    [SerializeField]
    List<KitchenObjectSO> validKitchenObjectSOArray;

    [SerializeField]
    Transform plateCompleteVisual;

    List<KitchenObjectSO> kitchenObjectSOArray;

    IKitchenObjectParent parent;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSo;
    }

    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOArray = new();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo)
    {
        if (kitchenObjectSOArray.Contains(kitchenObjectSo))
        {
            // Already has this ingredient
            return false;
        }

        if (!validKitchenObjectSOArray.Contains(kitchenObjectSo))
        {
            // Not a valid ingredient
            return false;
        }

        AddIngredientServerRpc(KitchenObjectManager.Instance.GetKitchenObjectSOIndex(kitchenObjectSo));

        return true;
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void AddIngredientServerRpc(int kitchenObjectIndex)
    {
        AddIngredientClientRpc(kitchenObjectIndex);
    }

    [ClientRpc]
    void AddIngredientClientRpc(int kitchenObjectIndex)
    {
        var kitchenObjectSo = KitchenObjectManager.Instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);

        kitchenObjectSOArray.Add(kitchenObjectSo);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs() { kitchenObjectSo = kitchenObjectSo });
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOArray;
    }
}
