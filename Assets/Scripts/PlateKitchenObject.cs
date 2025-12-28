using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    const float k_IngredientHeight = 0.1f;

    [SerializeField]
    List<KitchenObjectSO> validKitchenObjectSOArray;

    List<KitchenObjectSO> kitchenObjectSOArray;

    IKitchenObjectParent parent;

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

        var ingredient = Instantiate(kitchenObjectSo.prefab, transform, true);
        ingredient.transform.localPosition = Vector3.up * k_IngredientHeight * kitchenObjectSOArray.Count;
        return true;
    }
}
