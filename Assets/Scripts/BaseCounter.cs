using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField]
    private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public abstract void Interact(IKitchenObjectParent parent);

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
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
}
