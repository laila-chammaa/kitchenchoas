using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTimerUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ordersDeliveredText;

    void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    void OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            ordersDeliveredText.text = DeliverManager.Instance.GetOrdersDeliveredCount().ToString();
        }
        else
            Hide();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
