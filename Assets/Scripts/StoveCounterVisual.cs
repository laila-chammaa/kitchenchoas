using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField]
    GameObject[] visualGameObjects;

    [SerializeField]
    StoveCounter stoveCounter;

    void Start()
    {
        stoveCounter.OnFryingStateChanged += OnFryingStateChanged;
    }

    void OnFryingStateChanged(object sender, StoveCounter.OnFryingStateChangedEventArgs e)
    {
        if (e.isFrying)
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
