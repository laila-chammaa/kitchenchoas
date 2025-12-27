using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField]
    Transform platesPrefab;

    [SerializeField]
    protected Transform counterTopPoint;

    [SerializeField]
    PlatesCounter platesCounter;

    const float plateHeight = 0.1f;

    Stack<Transform> plates = new();

    void Start()
    {
        platesCounter.OnPlateSpawned += SpawnPlate;
        platesCounter.OnPlateTaken += RemovePlate;
    }

    void SpawnPlate(object sender, EventArgs e)
    {
        var plate = Instantiate(platesPrefab, counterTopPoint, true);
        plate.transform.localPosition = Vector3.up * plateHeight * plates.Count;
        plates.Push(plate);
    }

    void RemovePlate(object sender, EventArgs e)
    {
        // Destroy the last one
        Destroy(plates.Pop().gameObject);
    }
}
