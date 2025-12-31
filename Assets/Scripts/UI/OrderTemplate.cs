using System;
using TMPro;
using UnityEngine;

public class OrderTemplate : MonoBehaviour
{
    [SerializeField]
    Transform iconTemplate;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    Transform iconContainer;

    void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetOrder(RecipeSO recipeSo)
    {
        text.text = recipeSo.name;

        // For each ingredient, add icon
        foreach (var ingredient in recipeSo.ingredients)
        {
            // Create an iconTemplate with that icon
            var icon = Instantiate(iconTemplate, iconContainer);
            icon.gameObject.SetActive(true);
            icon.GetComponent<PlateIconTemplate>().SetIconImage(ingredient.sprite);
        }
    }
}
