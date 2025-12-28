using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField]
    PlateKitchenObject plateKitchenObject;

    [SerializeField]
    List<KitchenObjectSO_GameObject> entries;

    void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
    }

    void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var entry in entries)
        {
            if (entry.kitchenObjectSO == e.kitchenObjectSo)
            {
                entry.gameObject.SetActive(true);
            }
        }
    }
}
