using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField]
    KitchenObjectSO kitchenObjectSO;

    IKitchenObjectParent parent;

    public KitchenObjectSO GetKitchenObject()
    {
        return kitchenObjectSO;
    }

    public void SetParent(IKitchenObjectParent parent)
    {
        if (this.parent != null)
        {
            // changing parents
            this.parent.ClearKitchenObject();
        }

        this.parent = parent;
        if (parent.HasKitchenObject())
        {
            Debug.Log("Parent already has a KitchenObject!");
        }
        this.parent.SetKitchenObject(this);

        transform.parent = parent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetParent()
    {
        return parent;
    }
}
