using System;
using UnityEngine;

public class PlatesIconUI : MonoBehaviour
{
    [SerializeField]
    PlateKitchenObject plateKitchenObject;

    [SerializeField]
    Transform iconTemplate;

    void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
    }

    void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (Transform child in transform)
        {
            // Leave the first template
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (var kitchenObjectSo in plateKitchenObject.GetKitchenObjectSOList())
        {
            // Create an iconTemplate with that icon
            var icon = Instantiate(iconTemplate, transform);
            icon.gameObject.SetActive(true);
            icon.GetComponent<PlateIconTemplate>().SetIconImage(kitchenObjectSo.sprite);
        }
    }
}
