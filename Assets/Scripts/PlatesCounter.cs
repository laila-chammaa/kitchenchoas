using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField]
    Transform platesPrefab;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateTaken;

    int spawnedPlatesAmount;
    int spawnedPlatesAmountMax;

    float spawnTime; 
    const int k_SpawnTimeMax = 5;

    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= k_SpawnTimeMax)
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
