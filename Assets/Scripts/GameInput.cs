using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    PlayerInputAction playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += OnInteract;
        playerInputActions.Player.InteractAlternate.performed += OnInteractAlternate;
        playerInputActions.Player.Pause.performed += OnPause;
    }

    void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= OnInteract;
        playerInputActions.Player.InteractAlternate.performed -= OnInteractAlternate;
        playerInputActions.Player.Pause.performed -= OnPause;

        playerInputActions.Dispose();
    }

    void OnInteract(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    void OnInteractAlternate(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    void OnPause(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
