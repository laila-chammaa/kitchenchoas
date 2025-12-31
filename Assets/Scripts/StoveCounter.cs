using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnFry;

    public event EventHandler<OnFryingStateChangedEventArgs> OnFryingStateChanged;

    public class OnFryingStateChangedEventArgs : EventArgs
    {
        public bool isFrying;
    }

    [SerializeField]
    FryingRecipeSO[] fryingRecipeSoArray;

    float fryingTimer;
    bool isFrying = false;

    void Update()
    {
        if (GetKitchenObject() == null)
            return;

        var recipeSO = GetRecipeSO(GetKitchenObject().GetKitchenObjectSO());
        if (recipeSO == null)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
            if (isFrying)
            {
                isFrying = false;
                OnFryingStateChanged?.Invoke(this, new OnFryingStateChangedEventArgs { isFrying = false });
            }

            return;
        }

        fryingTimer += Time.deltaTime;

        if (!isFrying)
        {
            isFrying = true;
            OnFryingStateChanged?.Invoke(this, new OnFryingStateChangedEventArgs() { isFrying = true });
        }

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            { progressNormalized = fryingTimer / recipeSO.cookingTime });

        if (fryingTimer >= recipeSO.cookingTime)
        {
            GetKitchenObject().DestroySelf();

            // Spawn the next state item
            KitchenObject.SpawnKitchenObject(recipeSO.output.prefab, this);
            fryingTimer = 0;
        }
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject())
        {
            // If parent has object, drop it here
            if (parent.HasKitchenObject())
            {
                // Check if can be cooked
                var hasRecipe = HasRecipe(parent.GetKitchenObject().GetKitchenObjectSO());
                if (!hasRecipe)
                    return;

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
                // Reset frying timer on pick up
                fryingTimer = 0;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
                if (isFrying)
                {
                    isFrying = false;
                    OnFryingStateChanged?.Invoke(this, new OnFryingStateChangedEventArgs { isFrying = false });
                }
            }
        }
        else if (!parent.HasKitchenObject())
        {
            GetKitchenObject().SetParent(parent);

            // Reset frying timer on pick up
            fryingTimer = 0;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
            if (isFrying)
            {
                isFrying = false;
                OnFryingStateChanged?.Invoke(this, new OnFryingStateChangedEventArgs { isFrying = false });
            }
        }
    }

    bool HasRecipe(KitchenObjectSO kitchenObjectSo)
    {
        foreach (var recipe in fryingRecipeSoArray)
        {
            if (recipe.input.objectName.Equals(kitchenObjectSo.objectName))
                return true;
        }

        return false;
    }

    FryingRecipeSO GetRecipeSO(KitchenObjectSO input)
    {
        foreach (var recipe in fryingRecipeSoArray)
        {
            if (recipe.input.objectName == input.objectName)
                return recipe;
        }

        return null;
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
