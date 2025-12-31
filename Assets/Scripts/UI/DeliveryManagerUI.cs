using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField]
    Transform orderTemplate;

    void Awake()
    {
        orderTemplate.gameObject.SetActive(false);
    }

    void Start()
    {
        DeliverManager.Instance.OnOrdersChanged += DeliveryManagerOnOrdersChanged;
    }

    void DeliveryManagerOnOrdersChanged(object sender, EventArgs e)
    {
        foreach (Transform child in transform)
        {
            // Leave the first template
            if (child == orderTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (var order in DeliverManager.Instance.GetOrders())
        {
            // Create an orderTemplate with that icon
            var orderCard = Instantiate(orderTemplate, transform);
            orderCard.gameObject.SetActive(true);
            orderCard.GetComponent<OrderTemplate>().SetOrder(order);
        }

    }
}
