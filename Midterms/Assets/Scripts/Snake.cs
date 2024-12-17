using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private Vector3 targetPosition; // Next position on the grid
    private float moveSpeed = 5.0f; // Speed of movement (higher = faster)
    private List<Vector3> _positions; // Tracks all segment positions
    public List<Transform> _segments;
    public int tailValue = 0;
    
    private FoodValue foodValue;

    void Start()
    {
        targetPosition = transform.position;
        _positions = new List<Vector3> { targetPosition }; // Initialize with the snake head's position
        _segments = new List<Transform> { this.transform };
        foodValue = FindObjectOfType<FoodValue>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        // If the head has reached its target position, calculate the next grid-aligned position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition += new Vector3(_direction.x, _direction.y, 0);

            // Clamp the target position to within the grid bounds
            targetPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, 0, 8),
                Mathf.Clamp(targetPosition.y, 0, 8),
                0
            );

            _positions.Insert(0, targetPosition); // Add the new position to the list
            _positions.RemoveAt(_positions.Count - 1); // Remove the oldest position
        }

        // Move the snake's head smoothly towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

        // Clamp the head's position after movement
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, 0, 8),
            Mathf.Clamp(transform.position.y, 0, 8),
            0
        );

        // Update each segment's position to follow the segment ahead
        for (int i = 1; i < _segments.Count; i++)
        {
            _segments[i].position = Vector3.MoveTowards(
                _segments[i].position,
                _positions[i],
                moveSpeed * Time.fixedDeltaTime
            );

            // Clamp each segment's position
            _segments[i].position = new Vector3(
                Mathf.Clamp(_segments[i].position.x, 0, 8),
                Mathf.Clamp(_segments[i].position.y, 0, 8),
                0
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Food>() != null)
        {
            
            if (other.GetComponent<FoodValue>().foodValue > tailValue)
            {
                tailValue = other.GetComponent<FoodValue>().foodValue;
                other.transform.position = _positions[_positions.Count - 1]; // Place the new segment at the last segment's position
                Destroy(other.GetComponent<Food>());
                // other.GetComponent<SpriteRenderer>().color = Color.red;
                other.tag = "Segment";

                _segments.Add(other.transform); // Add the new segment to the list
                _positions.Add(other.transform.position); // Track its position
            }
            else
            {
                Debug.Log("Game Over");
            }
            
            
        }
    }

    public void ResetSnake()
    {
        _positions.Clear();
        _positions.Add(transform.position);

        _segments.Clear();
        _segments.Add(this.transform);

        tailValue = 0;

        Debug.Log("Snake reset: Positions and segments cleared.");
    }
}
