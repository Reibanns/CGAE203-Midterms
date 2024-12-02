using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private Vector3 targetPosition; // Next position on the grid
    private float moveSpeed = 5.0f; // Speed of movement (higher = faster)
    private List<Transform> _segments;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the target position to the current grid-aligned position
        targetPosition = transform.position;
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        // Handle input for direction changes
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

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = Vector3.MoveTowards(_segments[i - 1].position, targetPosition, moveSpeed * Time.deltaTime);
        }
        // If the snake has reached its target position, update to the next grid position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = new Vector3(
                Mathf.Round(targetPosition.x) + _direction.x,
                Mathf.Round(targetPosition.y) + _direction.y,
                0.0f
            );
        }

        // Smoothly move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = _segments[_segments.Count - 1].position;
        _segments.Add(other.transform);
        
    }
}