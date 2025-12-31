using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField]
    CuttingRecipeSO[] cuttingRecipeSoArray;

    public event EventHandler OnPlayerCutObject;
    public static event EventHandler OnCut;

    const int k_CuttingProgressMax = 3;
    int cuttingProgress;

    public override void Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject())
        {
            // If parent has object, drop it here
            if (parent.HasKitchenObject() && HasRecipe(parent.GetKitchenObject().GetKitchenObjectSO()))
            {
                var droppedObject = parent.GetKitchenObject();
                droppedObject.SetParent(this);
            }
        }
        // If the player is holding a plate, give ingredient to the player
        else if (parent.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();
                // Reset cutting progress on pick up
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
            }
        }
        else if (!parent.HasKitchenObject())
        {
            GetKitchenObject().SetParent(parent);

            // Reset cutting progress on pick up
            cuttingProgress = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0
            });
        }
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        if (cuttingProgress == k_CuttingProgressMax || 
            GetKitchenObject() == null || 
            !HasRecipe(GetKitchenObject().GetKitchenObjectSO()))
            return;

        // Replace the prefab
        var uncutKitchenObject = GetKitchenObject();

        cuttingProgress++;

        OnCut?.Invoke(this, EventArgs.Empty);
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
        {
            progressNormalized = (float)cuttingProgress/k_CuttingProgressMax
        });

        if (cuttingProgress >= k_CuttingProgressMax)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = 0 // Hide progress bar
            });

            var recipe = GetRecipeSO(uncutKitchenObject.GetKitchenObjectSO());
            uncutKitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(recipe.output.prefab, this);
            OnPlayerCutObject?.Invoke(this, EventArgs.Empty);
        }
    }

    CuttingRecipeSO GetRecipeSO(KitchenObjectSO input)
    {
        foreach (var recipe in cuttingRecipeSoArray)
        {
            if (recipe.input.objectName == input.objectName)
                return recipe;
        }

        return null;
    }

    bool HasRecipe(KitchenObjectSO kitchenObjectSo)
    {
        foreach (var recipe in cuttingRecipeSoArray)
        {
            if (recipe.input.objectName.Equals(kitchenObjectSo.objectName))
                return true;
        }

        return false;
    }
}
