using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField]
    KitchenObjectSO kitchenObjectSO;

    [SerializeField]
    Transform counterTopPoint;

    KitchenObject kitchenObject;

    public void Interact(IKitchenObjectParent parent)
    {
        if (kitchenObject == null)
        {
            // If parent has object, drop it here
            if (parent.HasKitchenObject())
            {
                var droppedObject = parent.GetKitchenObject();
                droppedObject.SetParent(this);
            }
            else
            {
                Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
                kitchenObjectTransform.GetComponent<KitchenObject>().SetParent(this);
            }
        }
        else
        {
            kitchenObject.SetParent(parent);
        }
    }

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
