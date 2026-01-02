using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliverManager : MonoBehaviour
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
        spawnOrderTimer += Time.deltaTime;
        if (GameManager.Instance.IsGamePlaying() && spawnOrderTimer >= k_SpawnOrderTimerMax && orders.Count < k_OrderMax)
        {
            spawnOrderTimer = 0;

            var order = recipeList.recipeList[Random.Range(0, recipeList.recipeList.Count)];
            orders.Add(order);
            OnOrdersChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (var order in orders)
        {
            if (order.ingredients.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Number of ingredients matches
                var plateContentMatchesRecipe = true;
                foreach (var ingredient in order.ingredients)
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
                    orders.Remove(order);
                    ordersDeliveredCount++;
                    spawnOrderTimer = 0;
                    OnOrdersChanged?.Invoke(this, EventArgs.Empty);
                    OnOrderDeliverySuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
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
