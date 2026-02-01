using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_WALKING = "IsWalking";
    
    [SerializeField]
    private Player player;
    
    private Animator animator;
    
    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsOwner && player != null)
        {
            animator.SetBool(IS_WALKING, player.IsWalking());
        }
    }
}
