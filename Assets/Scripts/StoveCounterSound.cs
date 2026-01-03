using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField]
    StoveCounter stoveCounter;

    AudioSource audioSource;

    float warningSoundTimer = 0;
    const float k_WarningSoundTimerMax = 0.2f;

    bool shouldPlayWarningSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        stoveCounter.OnFryingStateChanged += StoveCounterOnFryingStateChanged;
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
    }

    void Update()
    {
        warningSoundTimer += Time.deltaTime;
        if (shouldPlayWarningSound && warningSoundTimer >= k_WarningSoundTimerMax)
        {
            SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            warningSoundTimer = 0;
        }
    }

    void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        var burnShowProgressAmount = 0.5f;
        shouldPlayWarningSound = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsCooked();
    }

    void StoveCounterOnFryingStateChanged(object sender, StoveCounter.OnFryingStateChangedEventArgs e)
    {
        if (e.isFrying)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}
