using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField]
    protected KitchenObjectSO kitchenObjectSO;

    public override void Interact(IKitchenObjectParent parent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetParent(parent);
    }
}
