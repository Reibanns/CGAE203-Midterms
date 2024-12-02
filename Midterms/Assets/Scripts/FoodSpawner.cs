using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab; // The food prefab
    [SerializeField] private int gridWidth = 9;
    [SerializeField] private int gridHeight = 9;

    void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(0, gridWidth),
            Random.Range(0, gridHeight),
            0
        );

        // Instantiate food and get its Food component
        GameObject foodInstance = Instantiate(foodPrefab, randomPosition, Quaternion.identity);
        Food foodScript = foodInstance.GetComponent<Food>();

        // Assign the FoodSpawner reference
        foodScript.SetFoodSpawner(this);
    }
}