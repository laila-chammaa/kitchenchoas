using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    Button playButton;

    [SerializeField]
    Button quitButton;

    void Start()
    {
        playButton.onClick.AddListener(() => { SceneManager.LoadScene(1); });
        quitButton.onClick.AddListener(Application.Quit);
    }

}
