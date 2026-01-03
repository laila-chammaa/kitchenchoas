using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    [SerializeField]
    AudioClipsRefsSO audioClipsRefs;

    float volume = 1f;

    void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    void Start()
    {
        DeliverManager.Instance.OnOrderDeliverySuccess += DeliverManagerOnOnOrderDeliverySuccess;
        DeliverManager.Instance.OnOrderDeliveryFailure += DeliverManagerOnOnOrderDeliveryFailure;
        CuttingCounter.OnCut += CuttingCounterOnOnCut;
        BinCounter.OnTrash += BinCounterOnOnTrash;
        Player.Instance.OnObjectPickup += PlayerOnObjectPickup;
        BaseCounter.OnObjectDrop += CounterOnObjectDrop;
    }

    void PlayerOnObjectPickup(object sender, EventArgs e)
    {
        PlaySound(audioClipsRefs.objectPickup, Player.Instance.transform.position);
    }

    void CounterOnObjectDrop(object sender, EventArgs e)
    {
        var counter = sender as BaseCounter;
        PlaySound(audioClipsRefs.objectDrop, counter.transform.position);
    }

    void BinCounterOnOnTrash(object sender, EventArgs e)
    {
        var counter = sender as BaseCounter;
        PlaySound(audioClipsRefs.trash, counter.transform.position);
    }

    void CuttingCounterOnOnCut(object sender, EventArgs e)
    {
        var counter = sender as BaseCounter;
        PlaySound(audioClipsRefs.chop, counter.transform.position);
    }

    void DeliverManagerOnOnOrderDeliverySuccess(object sender, EventArgs e)
    {
        PlaySound(audioClipsRefs.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    void DeliverManagerOnOnOrderDeliveryFailure(object sender, EventArgs e)
    {
        PlaySound(audioClipsRefs.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    public void PlayFootstepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsRefs.footstep, position, volume);
    }

    public void PlayCountdownSound(float volume = 1f)
    {
        PlaySound(audioClipsRefs.warning, Vector3.zero, volume);
    }

    public void PlayWarningSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsRefs.warning, position, volume);
    }

    public void PlaySound(AudioClip[] audioClips, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Length)], position, volumeMultiplier * volume);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
