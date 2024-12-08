using System.Collections.Generic;
using UnityEngine;

public class GoalSpawner : MonoBehaviour
{
    public int xposition = 0, yposition = 0;
    [SerializeField] private List<GameObject> goals = new List<GameObject>();
    private GameObject currentGoal; // Reference to the currently active goal

    public void SpawnNewGoal()
    {
        // Destroy the old goal if it exists
        if (currentGoal != null)
        {
            Destroy(currentGoal);
            Debug.Log("Old goal destroyed.");
        }

        // Choose a new random goal
        int randomIndex = Random.Range(0, goals.Count);

        // Determine position based on goal type
        switch (randomIndex)
        {
            case 0: // Horizontal goal
                xposition = 4;
                yposition = Random.Range(0, 8);
                break;
            case 1: // Vertical goal
                xposition = Random.Range(0, 8);
                yposition = 4;
                break;
            case 2: // Square goal
                xposition = Random.Range(1, 7);
                yposition = Random.Range(1, 7);
                break;
            default:
                Debug.LogWarning("Unexpected goal index selected.");
                return;
        }

        // Instantiate the new goal and store a reference to it
        currentGoal = Instantiate(goals[randomIndex], new Vector3(xposition, yposition, 0), Quaternion.identity);
        Debug.Log("New goal spawned.");
    }

    private void Start()
    {
        SpawnNewGoal();
        // Spawn the initial goal
    }
}