using System;
using Unity.Netcode;
using UnityEngine;

public class KitchenObjectManager : NetworkBehaviour
{
    [SerializeField]
    KitchenObjectListSO m_KitchenObjectListSO; 
    public static KitchenObjectManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        var index = m_KitchenObjectListSO.kitchenObjectSOList.IndexOf(kitchenObjectSO);
        SpawnKitchenObjectServerRpc(index, parent.GetNetworkObject());
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void SpawnKitchenObjectServerRpc(int index, NetworkObjectReference kitchenObjectNetworkObject)
    {
        var prefab = m_KitchenObjectListSO.kitchenObjectSOList[index].prefab;
        Transform kitchenObjectTransform = Instantiate(prefab);
        kitchenObjectTransform.GetComponent<NetworkObject>().Spawn(true);

        kitchenObjectNetworkObject.TryGet(out var networkObject);
        var kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();
        kitchenObjectTransform.GetComponent<KitchenObject>().SetParent(kitchenObjectParent);
    }

}
