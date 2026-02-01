using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

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
