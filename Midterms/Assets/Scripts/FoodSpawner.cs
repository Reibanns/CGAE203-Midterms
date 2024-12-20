using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab; // The food prefab
    [SerializeField] private int gridWidth = 9;
    [SerializeField] private int gridHeight = 9;
    [SerializeField] private float spawnTimeCap = 300f; // 5 minutes maximum
    [SerializeField] private float minSpawnInterval = 0.5f; // Minimum interval between spawns

    public List<GameObject> foods = new List<GameObject>(); // The food prefabs for letters b to z
    private Queue<GameObject> spawnQueue = new Queue<GameObject>(); // Ordered shuffled queue
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // Track occupied grid spaces

    private float elapsedTime = 0f;
    private float spawnInterval = 3f; // Initial spawn interval

    void Start()
    {
        InitializeSpawnQueue();
        StartCoroutine(SpawnFoodWithIncreasingRate());
    }

    // Shuffle only the first 5 elements and enqueue everything in order
    private void InitializeSpawnQueue()
    {
        spawnQueue.Clear(); // Clear the queue before reinitializing
        List<GameObject> firstFive = foods.GetRange(0, Mathf.Min(5, foods.Count));
        ShuffleList(firstFive);

        // Enqueue the shuffled first 5 elements
        foreach (GameObject food in firstFive)
        {
            spawnQueue.Enqueue(food);
        }

        // Enqueue the rest of the elements in order
        for (int i = 5; i < foods.Count; i++)
        {
            spawnQueue.Enqueue(foods[i]);
        }
    }

    // Coroutine to spawn food over time with increasing rate
    private IEnumerator SpawnFoodWithIncreasingRate()
    {
        while (elapsedTime < spawnTimeCap)
        {
            if (spawnQueue.Count == 0)
            {
                // Reinitialize the queue if all foods have been spawned
                InitializeSpawnQueue();
            }

            // Spawn the next food object
            SpawnFood();

            // Increment elapsed time and decrease the spawn interval
            elapsedTime += spawnInterval;
            spawnInterval = Mathf.Max(minSpawnInterval, Mathf.Lerp(3f, minSpawnInterval, elapsedTime / spawnTimeCap));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnFood()
    {
        if (spawnQueue.Count == 0) return; // Ensure there are foods to spawn

        // Find a valid position that is not already occupied
        Vector3 randomPosition = Vector3.zero;
        int attempts = 0;
        do
        {
            randomPosition = new Vector3(
                Random.Range(0, gridWidth),
                Random.Range(0, gridHeight),
                0
            );
            attempts++;
        } while (occupiedPositions.Contains(randomPosition) && attempts < 100);

        if (attempts >= 100)
        {
            Debug.LogWarning("Could not find a free position to spawn food.");
            return;
        }

        // Mark the position as occupied
        occupiedPositions.Add(randomPosition);

        // Spawn the food at the selected position
        GameObject nextFood = spawnQueue.Dequeue(); // Get the next food object
        Instantiate(nextFood, randomPosition, Quaternion.identity);
    }

    // Fisher-Yates Shuffle Algorithm
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
