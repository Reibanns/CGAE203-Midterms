using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.2f;
    private Vector3 currentDirection = Vector3.zero;
    private bool isGameOver = false;
    
    public List<Transform> segments = new List<Transform>();

    void Start()
    {
        segments.Add(this.transform);
    }

    void Update()
    {
        if (isGameOver) return;
        
        if (Input.GetKeyDown(KeyCode.W) && currentDirection != Vector3.down)
            currentDirection = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.S) && currentDirection != Vector3.up)
            currentDirection = Vector3.down;
        else if (Input.GetKeyDown(KeyCode.A) && currentDirection != Vector3.right)
            currentDirection = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D) && currentDirection != Vector3.left)
            currentDirection = Vector3.right;
        
        if (currentDirection != Vector3.zero && !isMoving)
            StartCoroutine(MovePlayer(currentDirection));
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        Vector3[] previousPositions = new Vector3[segments.Count];
        for (int i = 0; i < segments.Count; i++)
        {
            previousPositions[i] = segments[i].position;
        }

        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            
            // Smoothly move each segment
            if (segments.Count >= 2)
            {
                for (int i = 1; i < segments.Count; i++)
                {
                    segments[i].position = Vector3.Lerp(segments[i].position, previousPositions[i - 1], elapsedTime / timeToMove);
                }
            }

            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        // Set final positions for each segment after movement is complete
        for (int i = 1; i < segments.Count; i++)
        {
            segments[i].position = previousPositions[i - 1];
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Debug.Log("Food Collected");
            Grow(other.gameObject); // Adds a new segment to the snake
            Destroy(other.gameObject); // Remove the food object from the scene
            FindObjectOfType<FoodSpawner>().SpawnFood();
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Game Over");
            currentDirection = Vector3.zero;
            isMoving = false;
            isGameOver = true;  // Set the game over flag to stop movement
        }
    }
    
    private void Grow(GameObject foodObject)
    {
        GameObject newSegment = new GameObject("Segment");
        
        newSegment.transform.position = segments[segments.Count - 1].position;
        SpriteRenderer newSegmentRenderer = newSegment.AddComponent<SpriteRenderer>();
        newSegmentRenderer.sprite = foodObject.GetComponent<SpriteRenderer>().sprite;
        newSegmentRenderer.sortingOrder = 1;

        segments.Add(newSegment.transform);
    }
}
