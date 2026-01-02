using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField]
    Transform platesPrefab;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateTaken;

    int spawnedPlatesAmount;
    float spawnTime; 

    const int k_SpawnedPlatesAmountMax = 5;
    const int k_SpawnTimeMax = 4;

    void Update()
    {
        spawnTime += Time.deltaTime;
        if (GameManager.Instance.IsGamePlaying() && spawnTime >= k_SpawnTimeMax && spawnedPlatesAmount <= k_SpawnedPlatesAmountMax)
        {
            spawnedPlatesAmount++;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            spawnTime = 0;
        }
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (spawnedPlatesAmount > 0 && !parent.HasKitchenObject())
        {
            // Actually create the SO and set its parent to the player
            KitchenObject.SpawnKitchenObject(platesPrefab, parent);
            spawnedPlatesAmount--;
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
            spawnTime = 0;
        }
    }                                  

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        // Empty on purpose
    }
}
