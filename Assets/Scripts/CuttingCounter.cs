using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnPlayerCutObject;
    const int k_CuttingProgressMax = 3;
    int cuttingProgress;

    public override void Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject())
        {
            // If parent has object, drop it here
            if (parent.HasKitchenObject())
            {
                var droppedObject = parent.GetKitchenObject();
                droppedObject.SetParent(this);
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
            GetKitchenObject().GetKitchenObjectSO().cutPrefab == null)
            return;

        // Replace the prefab
        var uncutKitchenObject = GetKitchenObject();
        var cutPrefab = uncutKitchenObject.GetKitchenObjectSO().cutPrefab;

        cuttingProgress++;

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

            uncutKitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(cutPrefab, this);
            OnPlayerCutObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
