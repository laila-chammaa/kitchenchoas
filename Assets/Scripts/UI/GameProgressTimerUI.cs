using System;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressTimerUI : MonoBehaviour
{
    [SerializeField]
    Image timerProgressImage;

    void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    void Update()
    {
        timerProgressImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }

    void OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
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
