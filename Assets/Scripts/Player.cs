using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f;

    [SerializeField]
    GameInput gameInput;

    [SerializeField]
    LayerMask countersLayerMask;

    Vector3 lastInteractDir;
    bool isWalking;

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    void HandleInteraction()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();
        var moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        var interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out var raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter
                clearCounter.Interact();
            }
        }
    }

    void HandleMovement()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();
        var moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        var playerRadius = 0.7f;
        var playerHeight = 2f;
        var moveDistance = moveSpeed * Time.deltaTime;
        var willCollide = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if (willCollide) 
        {
            // Attempt just X movement
            var moveX = new Vector3(moveDir.x, 0, 0).normalized;
            willCollide = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveX, moveDistance);
            if (!willCollide)
            {
                moveDir = moveX;
            }
            else
            {
                // Attempt just Z movement
                var moveZ = new Vector3(0, 0, moveDir.z).normalized;
                willCollide = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveZ, moveDistance);
                if (!willCollide)
                {
                    moveDir = moveZ;
                }
            }
        }
        
        if (!willCollide)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        var rotationSpeed = 10f;
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
