using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField]
    KitchenObjectSO kitchenObjectSO;

    IKitchenObjectParent parent;

    public KitchenObjectSO GetKitchenObjectSO()
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
            return;
        }
        this.parent.SetKitchenObject(this);

        transform.parent = parent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetParent()
    {
        return parent;
    }

    public void DestroySelf()
    {
        parent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        KitchenObjectManager.Instance.SpawnKitchenObject(kitchenObjectSO, parent);
    }
}
