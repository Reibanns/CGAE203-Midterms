using UnityEngine;

public class Goal : MonoBehaviour
{
    public int totalSegments = -1, count = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            return;
        }
        // Update total segments
        totalSegments = GameObject.FindGameObjectsWithTag("Segment").Length;

        if (other.CompareTag("Segment"))
        {
            count++;
            Debug.Log("Segment Entered");
        }

        // Check if all segments have entered
        if (count == totalSegments)
        {
            Debug.Log("All segments entered. Clearing segments...");

            // Find all segments and destroy them
            GameObject[] segments = GameObject.FindGameObjectsWithTag("Segment");
            foreach (GameObject segment in segments)
            {
                Destroy(segment);
            }

            Snake snake = FindObjectOfType<Snake>();
            if (snake != null)
            {
                snake.ResetSnake(); // Reset the snake state after clearing
            }

            // Spawn a new goal using the GoalSpawner
            GoalSpawner spawner = FindObjectOfType<GoalSpawner>();
            if (spawner != null)
            {
                spawner.SpawnNewGoal();
            }
            else
            {
                Debug.LogError("No GoalSpawner found in the scene.");
            }

            // Reset counters
            count = 0;
            totalSegments = -1;

            Debug.Log("Segments cleared and new goal spawned.");
        }
    }
}