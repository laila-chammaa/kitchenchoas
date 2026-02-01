using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]
    BaseCounter counter;

    [SerializeField]
    GameObject[] visualGameObjects;

    void Start()
    {
        // Player.Instance.OnSelectedCounterChanged += OnSelectedCounterChanged;
    }

    void OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    void Show()
    {
        foreach (var go in visualGameObjects)
        {
            go.SetActive(true);
        }
    }

    void Hide()
    {
        foreach (var go in visualGameObjects)
        {
            go.SetActive(false);
        }
    }
}
