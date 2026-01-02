using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static EventHandler OnObjectDrop;

    [SerializeField]
    private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public abstract void Interact(IKitchenObjectParent parent);
    public abstract void InteractAlternate(IKitchenObjectParent parent);

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnObjectDrop?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        OnObjectDrop = null;
    }
}
