using System;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField]
    KitchenObjectSO kitchenObjectSO;

    IKitchenObjectParent parent;

    FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetParent(IKitchenObjectParent parent)
    {
        SetParentServerRpc(parent.GetNetworkObject());
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void SetParentServerRpc(NetworkObjectReference networkObjectReference)
    {
        SetParentClientRpc(networkObjectReference);
    }

    [ClientRpc]
    void SetParentClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out var networkObject);
        var kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();

        if (parent != null)
        {
            // changing parents
            parent.ClearKitchenObject();
        }

        parent = kitchenObjectParent;
        if (parent.HasKitchenObject())
        {
            Debug.Log("Parent already has a KitchenObject!");
            return;
        }
        parent.SetKitchenObject(this);

        followTransform.SetTargetTransform(parent.GetKitchenObjectFollowTransform());
    }

    public IKitchenObjectParent GetParent()
    {
        return parent;
    }

    public void DestroySelf()
    {
        ClearParentServerRpc(parent.GetNetworkObject());
        DestroyKitchenObject(this);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void ClearParentServerRpc(NetworkObjectReference networkObjectReference)
    {
        ClearParentClientRpc(networkObjectReference);
    }

    [ClientRpc]
    void ClearParentClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out var networkObject);
        var kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();
        kitchenObjectParent.ClearKitchenObject();
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        KitchenObjectManager.Instance.SpawnKitchenObject(kitchenObjectSO, parent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenObjectManager.Instance.DestroyKitchenObject(kitchenObject);
    }
}
