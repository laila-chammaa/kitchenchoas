using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    enum FryingState
    {
        Uncooked,
        Cooked,
        Burnt
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField]
    FryingRecipeSO[] fryingRecipeSoArray;

    float fryingTimer;
    FryingState currentState;

    void Update()
    {
        if (GetKitchenObject() == null)
            return;

        fryingTimer += Time.deltaTime;

        var recipeSO = GetRecipeSO(GetKitchenObject().GetKitchenObjectSO());

        if (recipeSO == null)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = 0
            });
            return;
        }

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
        {
            progressNormalized = fryingTimer/recipeSO.cookingTime
        });

        if (fryingTimer >= recipeSO.cookingTime)
        {
            GetKitchenObject().DestroySelf();
            // Spawn the next state item
            KitchenObject.SpawnKitchenObject(recipeSO.output.prefab, this);
            currentState = GetNextFryingState(currentState);
            fryingTimer = 0;
        }
    }

    static FryingState GetNextFryingState(FryingState state)
    {
        switch (state)
        {
            case FryingState.Uncooked:
                return FryingState.Cooked;
            case FryingState.Cooked:
                return FryingState.Burnt;
            case FryingState.Burnt:
                return FryingState.Burnt;
        }

        Debug.LogWarning("Reached strange state.");
        return FryingState.Burnt;
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
                // currentState = GetCurrentStateFromKitchenObject(GetKitchenObject());
            }
        }
        else if (!parent.HasKitchenObject())
        {
            GetKitchenObject().SetParent(parent);

            // Reset frying timer on pick up
            fryingTimer = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = 0
            });
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

    // FryingState GetCurrentStateFromKitchenObject(KitchenObject kitchenObject)
    // {
    //     foreach (var recipe in fryingRecipeSoArray)
    //     {
    //         if (recipe.input.n)
    //     }
    //     kitchenObject.GetKitchenObjectSO().objectName
    // }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Start cooking
    }
}
