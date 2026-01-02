using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField]
    Button resumeButton;

    [SerializeField]
    Button optionsButton;

    [SerializeField]
    Button mainMenuButton;

    void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += OnStateChanged;
        resumeButton.onClick.AddListener(() => GameManager.Instance.TogglePauseGame());
        optionsButton.onClick.AddListener(() => OptionsMenuUI.Instance.Show());
        mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    void OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePaused())
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
