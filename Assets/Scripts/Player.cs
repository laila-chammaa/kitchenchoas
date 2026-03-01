using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static Player LocalInstance;

    public static event EventHandler OnAnyPlayerSpawned;
    public event EventHandler OnObjectPickup;
    public static event EventHandler OnAnyObjectPickup;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    
    [SerializeField]
    private float moveSpeed = 7f;

    [SerializeField]
    LayerMask collisionLayerMask;

    [SerializeField]
    Transform playerHandsPoint;

    [SerializeField]
    List<Vector3> spawnPointsList;

    Vector3 lastInteractDir;
    bool isWalking;
    BaseCounter selectedCounter;
    KitchenObject kitchenObject;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;

            transform.position = spawnPointsList[(int)OwnerClientId];

            OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Start()
    {
        GameInput.Instance.OnInteractAction += GameInputOnOnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInputOnOnInteractAlternateAction;
    }

    void GameInputOnOnInteractAction(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            selectedCounter?.Interact(this);
        }
    }

    void GameInputOnOnInteractAlternateAction(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            selectedCounter?.InteractAlternate(this);
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMovement();
        HandleInteraction();
    }

    void HandleInteraction()
    {
        var inputVector = GameInput.Instance.GetMovementVectorNormalized();
        var moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        var interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out var raycastHit, interactDistance, collisionLayerMask))
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
        var inputVector = GameInput.Instance.GetMovementVectorNormalized();
        var moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        var playerRadius = 0.7f;
        var playerHeight = 2f;
        var moveDistance = moveSpeed * Time.deltaTime;
        var willCollide = Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionLayerMask);
        
        if (willCollide) 
        {
            // Attempt just X movement
            var moveX = new Vector3(moveDir.x, 0, 0).normalized;
            willCollide = moveDir.x == 0 || Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveX, Quaternion.identity, moveDistance, collisionLayerMask);
            if (!willCollide)
            {
                moveDir = moveX;
            }
            else
            {
                // Attempt just Z movement
                var moveZ = new Vector3(0, 0, moveDir.z).normalized;
                willCollide = moveDir.z == 0 || Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveZ, Quaternion.identity, moveDistance, collisionLayerMask);
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

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnObjectPickup?.Invoke(this, EventArgs.Empty);
            OnAnyObjectPickup?.Invoke(this, EventArgs.Empty);
        }
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

    private static void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        OnAnyPlayerSpawned = null;
        OnAnyObjectPickup = null;
    }
}
