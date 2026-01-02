using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    public static OptionsMenuUI Instance;

    [SerializeField]
    Button closeButton;

    [SerializeField]
    Slider musicVolumeSlider;

    [SerializeField]
    Slider soundEffectsVolumeSlider;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Hide();
        UpdateVisual();
        closeButton.onClick.AddListener(Hide);
        musicVolumeSlider.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance.SetVolume(value);
        });

        soundEffectsVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.SetVolume(value);
        });
    }

    void UpdateVisual()
    {
        musicVolumeSlider.value = MusicManager.Instance.GetVolume();
        soundEffectsVolumeSlider.value = SoundManager.Instance.GetVolume();
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
