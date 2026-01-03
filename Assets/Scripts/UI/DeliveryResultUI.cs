using System;
using UnityEngine;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField]
    Transform successUI;

    [SerializeField]
    Transform failureUI;

    const string POPUP = "Popup";

    float showResultTimer;
    const float k_ShowResultTimerMax = 1f;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        DeliverManager.Instance.OnOrderDeliverySuccess += OnOrderDeliverySuccess;
        DeliverManager.Instance.OnOrderDeliveryFailure += OnOrderDeliveryFailure;
        Hide(successUI);
        Hide(failureUI);
    }

    void Update()
    {
        showResultTimer -= Time.deltaTime;
        if (showResultTimer < 0)
        {
            Hide(successUI);
            Hide(failureUI);
        }
    }

    void OnOrderDeliverySuccess(object sender, EventArgs e)
    {
        showResultTimer = k_ShowResultTimerMax;
        animator.SetTrigger(POPUP);
        Show(successUI);
    }

    void OnOrderDeliveryFailure(object sender, EventArgs e)
    {
        showResultTimer = k_ShowResultTimerMax;
        animator.SetTrigger(POPUP);
        Show(failureUI);
    }

    void Show(Transform uiGameObject)
    {
        uiGameObject.gameObject.SetActive(true);
    }

    void Hide(Transform uiGameObject)
    {
        uiGameObject.gameObject.SetActive(false);
    }
}
