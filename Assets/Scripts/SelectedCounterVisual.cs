using System;
using Unity.Netcode;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]
    BaseCounter counter;

    [SerializeField]
    GameObject[] visualGameObjects;

    void Start()
    {
        if (Player.LocalInstance != null)
        { 
            Player.LocalInstance.OnSelectedCounterChanged += OnSelectedCounterChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += OnAnyPlayerSpawned;
        }
    }

    void OnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged -= OnSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += OnSelectedCounterChanged;
        }
    }

    void OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (counter == null)
        {
            Debug.LogWarning("Selected counter visual has a null counter reference. Very strange indeed.");
            return;
        }

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
