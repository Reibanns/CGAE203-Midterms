using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int totalSegments = -1;
    private HashSet<Collider2D> enteredSegments = new HashSet<Collider2D>(); // Track unique colliders
    private ScoreScript sc;

    void Start()
    {
        sc = GameObject.Find("ScoreManager").GetComponent<ScoreScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        // Update total segments count
        totalSegments = GameObject.FindGameObjectsWithTag("Segment").Length;

        if (other.CompareTag("Segment"))
        {
            if (!enteredSegments.Contains(other)) // Prevent duplicate entries
            {
                enteredSegments.Add(other);
                Debug.Log("Segment Entered");
            }
        }

        CheckAllSegmentsEntered();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Segment"))
        {
            if (enteredSegments.Contains(other)) // Remove only tracked segments
            {
                enteredSegments.Remove(other);
                Debug.Log("Segment Exited");
            }
        }
    }

    private void CheckAllSegmentsEntered()
    {
        if (enteredSegments.Count == totalSegments)
        {
            Debug.Log("All segments entered. Clearing segments...");
            sc.AddScore(enteredSegments.Count);
            
            // Destroy all segments
            GameObject[] segments = GameObject.FindGameObjectsWithTag("Segment");
            foreach (GameObject segment in segments)
            {
                Destroy(segment);
            }

            // Reset the snake state
            Snake snake = FindObjectOfType<Snake>();
            if (snake != null)
            {
                snake.ResetSnake();
            }

            // Spawn a new goal
            GoalSpawner spawner = FindObjectOfType<GoalSpawner>();
            if (spawner != null)
            {
                spawner.SpawnNewGoal();
            }
            else
            {
                Debug.LogError("No GoalSpawner found in the scene.");
            }

            // Update the score
            

            // Reset everything
            enteredSegments.Clear();
            totalSegments = -1;

            Debug.Log("Segments cleared and new goal spawned.");
        }
    }
}
