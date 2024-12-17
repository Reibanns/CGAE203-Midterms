using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private FoodSpawner _foodSpawner;

    // Method to assign the FoodSpawner reference
    public void SetFoodSpawner(FoodSpawner foodSpawner)
    {
        _foodSpawner = foodSpawner;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            // Debug.Log("Player entered");
            // _foodSpawner.SpawnFood(); // Call SpawnFood on the assigned spawner
            // Destroy(gameObject); // Destroy the current food
        }
    }
}