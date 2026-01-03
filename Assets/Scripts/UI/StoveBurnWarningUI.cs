using System;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField]
    StoveCounter stoveCounter;

    const string IS_FLASHING = "IsFlashing";

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnOnProgressChanged;
        Hide();
    }

    void StoveCounterOnOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        var burnShowProgressAmount = 0.5f;
        var show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsCooked();
        if (show)
        {
            Show();
            animator.SetBool(IS_FLASHING, true);
        }
        else
        {
            Hide();
        }
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