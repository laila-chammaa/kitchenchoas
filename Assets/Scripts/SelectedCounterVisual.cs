using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]
    ClearCounter clearCounter;

    [SerializeField]
    GameObject visualGameObject;

    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += OnSelectedCounterChanged;
    }

    void OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == clearCounter)
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
        visualGameObject.SetActive(true);
    }

    void Hide()
    {
        visualGameObject.SetActive(false);
    }
}
