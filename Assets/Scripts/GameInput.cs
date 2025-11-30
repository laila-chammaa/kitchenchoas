using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;

    PlayerInputAction playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += OnInteract;
    }

    void OnInteract(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
