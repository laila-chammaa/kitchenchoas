using System;
using System.Collections.Generic;
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

    void Awake()
    {
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

        kitchenObjectSOArray.Add(kitchenObjectSo);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs() { kitchenObjectSo = kitchenObjectSo });
        
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOArray;
    }
}
