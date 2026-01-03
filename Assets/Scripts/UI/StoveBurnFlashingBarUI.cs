using System;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
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
        animator.SetBool(IS_FLASHING, false);
    }

    void StoveCounterOnOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        var burnShowProgressAmount = 0.5f;
        var show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsCooked();
        animator.SetBool(IS_FLASHING, show);
    }
}