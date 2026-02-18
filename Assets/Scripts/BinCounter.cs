using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BinCounter : BaseCounter
{
    public static event EventHandler OnTrash;

    void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    public override void Interact(IKitchenObjectParent parent)
    {
        parent.GetKitchenObject().DestroySelf();
        InteractServerRpc();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void InteractServerRpc()
    {
        InteractClientRpc();
    }

    [ClientRpc]
    void InteractClientRpc()
    {
        OnTrash?.Invoke(this, EventArgs.Empty);
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }

    void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        OnTrash = null;
    }
}
