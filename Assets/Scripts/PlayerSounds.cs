using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    Player player;

    float footStepsTimer;
    const float k_FootstepsTimerMax = 0.1f;

    void Awake()
    {
        player = GetComponent<Player>();
    } 

    void Update()
    {
        footStepsTimer += Time.deltaTime;
        if (footStepsTimer >= k_FootstepsTimerMax && player.IsWalking())
        {
            footStepsTimer = 0;
            SoundManager.Instance.PlayFootstepsSound(player.transform.position);
        }
    }
}
