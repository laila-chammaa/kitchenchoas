using UnityEngine;

public class KitchenObject : MonoBehaviour
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

    public static Transform SpawnKitchenObject(Transform prefab, IKitchenObjectParent parent)
    {
        Transform kitchenObjectTransform = Instantiate(prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetParent(parent);
        return kitchenObjectTransform;
    }
}
