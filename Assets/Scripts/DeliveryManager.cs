using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliverManager : NetworkBehaviour
{
    public static DeliverManager Instance { get; private set; }

    [SerializeField]
    RecipeListSO recipeList;

    List<RecipeSO> orders;

    int ordersDeliveredCount = 0;

    float spawnOrderTimer;

    const float k_SpawnOrderTimerMax = 10f;
    const int k_OrderMax = 4;

    public event EventHandler OnOrdersChanged;
    public event EventHandler OnOrderDeliverySuccess;
    public event EventHandler OnOrderDeliveryFailure;

    void Awake()
    {
        Instance = this;
        orders = new List<RecipeSO>();
    }

    void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnOrderTimer += Time.deltaTime;
        if (GameManager.Instance.IsGamePlaying() && spawnOrderTimer >= k_SpawnOrderTimerMax && orders.Count < k_OrderMax)
        {
            spawnOrderTimer = 0;

            var recipeIndex = Random.Range(0, recipeList.recipeList.Count);
            SpawnRecipeClientRpc(recipeIndex);
        }
    }

    [ClientRpc]
    void SpawnRecipeClientRpc(int recipeIndex)
    {
        var order = recipeList.recipeList[recipeIndex];
        orders.Add(order);
        OnOrdersChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (var i = 0; i < orders.Count; i++)
        {
            if (orders[i].ingredients.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Number of ingredients matches
                var plateContentMatchesRecipe = true;
                foreach (var ingredient in orders[i].ingredients)
                {
                    var ingredientFound = false;
                    foreach (var kitchenObject in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (kitchenObject == ingredient)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    // Order was fulfilled!
                    DeliverOrderSuccessServerRpc(i);
                    return;
                }
            }
        }

        DeliverOrderFailedServerRpc();
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void DeliverOrderSuccessServerRpc(int orderIndex)
    {
        DeliverOrderSuccessClientRpc(orderIndex);
    }

    [ClientRpc]
    void DeliverOrderSuccessClientRpc(int orderIndex)
    {
        orders.RemoveAt(orderIndex);
        ordersDeliveredCount++;
        spawnOrderTimer = 0;
        OnOrdersChanged?.Invoke(this, EventArgs.Empty);
        OnOrderDeliverySuccess?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void DeliverOrderFailedServerRpc()
    {
        DeliverOrderFailedClientRpc();
    }

    [ClientRpc]
    void DeliverOrderFailedClientRpc()
    {
        OnOrderDeliveryFailure?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetOrders()
    {
        return orders;
    }

    public int GetOrdersDeliveredCount()
    {
        return ordersDeliveredCount;
    }
}
