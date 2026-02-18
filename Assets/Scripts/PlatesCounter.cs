using System;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField]
    KitchenObjectSO platesSO;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateTaken;

    int spawnedPlatesAmount;
    float spawnTime; 

    const int k_SpawnedPlatesAmountMax = 5;
    const int k_SpawnTimeMax = 4;

    void Update()
    {
        if (!IsServer)
            return;

        spawnTime += Time.deltaTime;
        if (GameManager.Instance.IsGamePlaying() && spawnTime >= k_SpawnTimeMax && spawnedPlatesAmount <= k_SpawnedPlatesAmountMax)
        {
            SpawnPlateServerRpc();
        }
    }

    [ServerRpc]
    void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    void SpawnPlateClientRpc() 
    {
        spawnedPlatesAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        spawnTime = 0;
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (spawnedPlatesAmount > 0 && !parent.HasKitchenObject())
        {
            // Actually create the SO and set its parent to the player
            KitchenObject.SpawnKitchenObject(platesSO, parent);
            InteractServerRpc();
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void InteractServerRpc()
    {
        InteractClientRpc();
    }

    [ClientRpc]
    void InteractClientRpc()
    {
        spawnedPlatesAmount--;
        OnPlateTaken?.Invoke(this, EventArgs.Empty);
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
