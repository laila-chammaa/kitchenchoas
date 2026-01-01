using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI countDownText;

    void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    void Update()
    {
        countDownText.text = GameManager.Instance.GetGameStartCountdown().ToString();
    }

    void OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameStartCountDownActive())
            Show();
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
