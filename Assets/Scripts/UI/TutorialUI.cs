using System;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
        Show();
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
