using System;
using UnityEngine;

public class WaitingForPlayersUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
        GameManager.Instance.OnLocalPlayerReady += OnLocalPlayerReady;
        Hide();
    }

    void OnLocalPlayerReady(object sender, EventArgs e)
    {
        if (GameManager.Instance.localPlayerReady)
        {
            Show();
        }
    }

    void OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameStartCountDownActive())
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
