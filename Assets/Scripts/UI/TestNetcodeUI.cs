using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class TestNetcodeUI : MonoBehaviour
{
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var hostButton = root.Q<Button>("host");
        var clientButton = root.Q<Button>("client");

        hostButton.clicked += () =>
        {
            NetworkManager.Singleton.StartHost();
            Hide();
        };

        clientButton.clicked += () =>
        {
            NetworkManager.Singleton.StartClient();
            Hide();
        };
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}

// Tested new features in 6000.5, worked great, but I want to work on the LTS
/*
[RequireComponent(typeof(PanelRenderer))]
public class TestNetcodeUI : MonoBehaviour
{
    [SerializeField]
    VisualElementReference<Button> hostButton;

    [SerializeField]
    VisualElementReference<Button> clientButton;

    void OnEnable()
    {
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
    }

    void OnUIReload(PanelRenderer panelRenderer, VisualElement rootElement)
    {
        hostButton.RegisterReferenceResolvedCallback((button) =>
        {
            button.clicked += () =>
            {
                NetworkManager.Singleton.StartHost();
                Hide();
            };
        });

        clientButton.RegisterReferenceResolvedCallback((button) =>
        {
            button.clicked += () =>
            {
                NetworkManager.Singleton.StartClient();
                Hide();
            };
        });
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
*/
