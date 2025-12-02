using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    
    [SerializeField]
    private float moveSpeed = 7f;

    [SerializeField]
    GameInput gameInput;

    [SerializeField]
    LayerMask countersLayerMask;

    [SerializeField]
    Transform playerHandsPoint;

    Vector3 lastInteractDir;
    bool isWalking;
    BaseCounter selectedCounter;
    KitchenObject kitchenObject;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }

        Instance = this;
    }

    void Start()
    {
        gameInput.OnInteractAction += GameInputOnOnInteractAction;
    }

    void GameInputOnOnInteractAction(object sender, EventArgs e)
    {
        selectedCounter?.Interact(this);
    }

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
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                SetSelectedCounter(counter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
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
            var oldRotation = transform.forward;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter counter)
    {
        if (selectedCounter != counter)
        {
            selectedCounter = counter;
            OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return playerHandsPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
