using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BinCounter : BaseCounter
{
    public static event EventHandler OnTrash;

    void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    public override void Interact(IKitchenObjectParent parent)
    {
        OnTrash?.Invoke(this, EventArgs.Empty);
        parent.GetKitchenObject().DestroySelf();
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }

    void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        OnTrash = null;
    }
}
