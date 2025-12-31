using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField]
    StoveCounter stoveCounter;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        stoveCounter.OnFryingStateChanged += StoveCounterOnOnFryingStateChanged;
    }

    void StoveCounterOnOnFryingStateChanged(object sender, StoveCounter.OnFryingStateChangedEventArgs e)
    {
        if (e.isFrying)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}
