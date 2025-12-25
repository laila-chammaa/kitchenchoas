using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler OnPlayerCutObject;
    public event EventHandler<OnCuttingProgressChangedEventArgs> OnCuttingProgressChanged;

    public class OnCuttingProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    const int cuttingProgressMax = 3;
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
        }
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        if (cuttingProgress == cuttingProgressMax || 
            GetKitchenObject() == null || 
            GetKitchenObject().GetKitchenObjectSO().cutPrefab == null)
            return;

        // Replace the prefab
        var uncutKitchenObject = GetKitchenObject();
        var cutPrefab = uncutKitchenObject.GetKitchenObjectSO().cutPrefab;

        cuttingProgress++;

        OnCuttingProgressChanged.Invoke(this, new OnCuttingProgressChangedEventArgs()
        {
            progressNormalized = (float)cuttingProgress/cuttingProgressMax
        });

        if (cuttingProgress >= cuttingProgressMax)
        {
            OnCuttingProgressChanged.Invoke(this, new OnCuttingProgressChangedEventArgs()
            {
                progressNormalized = 0 // Hide progress bar
            });

            uncutKitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(cutPrefab, this);
            OnPlayerCutObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
