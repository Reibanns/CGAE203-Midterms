using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetTilePrefab; // The prefab for the target tiles
    private HashSet<Vector3> targetTilePositions = new HashSet<Vector3>(); // Positions for target tiles
    private List<GameObject> activeTargetTiles = new List<GameObject>(); // Stores active target tile prefabs

    private PlayerMovement playerMovement;// Reference to snake segments
    
    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        // Check if the reference is valid
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on the player object.");
            return;
        }
        
        InitializeTargetTiles();
        // Assume segments are initialized elsewhere, or find a way to reference them
    }

    private void Update()
    {

        if (playerMovement.segments.Count > 1)
        {
            Debug.Log("thit"+ playerMovement.segments.Count.ToString());
            CheckAndDestroyOverlappingSegments();
        }
            
    }

    private void InitializeTargetTiles()
    {
        // Clear previous target tiles and destroy their game objects
        foreach (GameObject tile in activeTargetTiles)
        {
            Destroy(tile);
        }
        activeTargetTiles.Clear();
        targetTilePositions.Clear();

        // Randomly select a shape configuration
        int shapeType = Random.Range(0, 3); // 0: horizontal line, 1: vertical line, 2: 3x3 square
        Vector3 startPosition;

        switch (shapeType)
        {
            case 0: // Horizontal line
                // Randomly choose a starting position that allows 9 tiles horizontally within the grid width of 9
                startPosition = new Vector3(Random.Range(0, 1), Random.Range(0, 9), 0);
                for (int i = 0; i < 9; i++)
                {
                    Vector3 tilePosition = startPosition + new Vector3(i, 0, 0);
                    SpawnTargetTile(tilePosition);
                }
                break;

            case 1: // Vertical line
                // Randomly choose a starting position that allows 9 tiles vertically within the grid height of 9
                startPosition = new Vector3(Random.Range(0, 9), Random.Range(0, 1), 0);
                for (int i = 0; i < 9; i++)
                {
                    Vector3 tilePosition = startPosition + new Vector3(0, i, 0);
                    SpawnTargetTile(tilePosition);
                }
                break;

            case 2: // 3x3 square
                // Randomly choose a starting position that allows a 3x3 square within the grid
                startPosition = new Vector3(Random.Range(0, 7), Random.Range(0, 7), 0);
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        Vector3 tilePosition = startPosition + new Vector3(x, y, 0);
                        SpawnTargetTile(tilePosition);
                    }
                }
                break;
        }

        // Debug.Log("Target Tile Shape Initialized: " + (shapeType == 0 ? "Horizontal Line" : shapeType == 1 ? "Vertical Line" : "3x3 Square"));
    }


    private void SpawnTargetTile(Vector3 position)
    {
        GameObject tile = Instantiate(targetTilePrefab, position, Quaternion.identity); // Instantiate prefab at position
        activeTargetTiles.Add(tile); // Keep track of the tile for later destruction
        targetTilePositions.Add(position); // Store the position in the set
        // Debug.Log("Target Tile Spawned: " + position.ToString());
    }

    private void CheckAndDestroyOverlappingSegments()
    {
        // Define a tolerance range for checking overlaps
        float tolerance = 0.5f;

        HashSet<Vector3> coveredTiles = new HashSet<Vector3>();
        

        // Ensure we start from the second segment
        for (int i = 1; i < playerMovement.segments.Count; i++) 
        {
            Vector3 segmentPosition = playerMovement.segments[i].position;
            // Debug.Log("Segment Position: " + segmentPosition.ToString());

            // Check if the segment is within the tolerance range of any target tile position
            foreach (Vector3 targetPosition in targetTilePositions)
            {
                if (Vector3.Distance(segmentPosition, targetPosition) <= tolerance)
                {
                    coveredTiles.Add(targetPosition); // Mark tile as covered
                    break; // No need to check further if we've already found a matching tile
                }
            }
        }

        // If all target tiles are covered, destroy overlapping segments and respawn new target tiles
        if (coveredTiles.Count == targetTilePositions.Count)
        {
            DestroyOverlappingSegments();
            InitializeTargetTiles(); // Respawn new target tiles with a different shape
        }
    }

    private void DestroyOverlappingSegments()
    {
        for (int i = 1; i < playerMovement.segments.Count; i++)
        {
            Vector3 segmentPosition = playerMovement.segments[i].position;
            if (targetTilePositions.Contains(segmentPosition))
            {
                Destroy(playerMovement.segments[i].gameObject);
            }
        }

        // Destroy all current target tile objects
        foreach (GameObject tile in activeTargetTiles)
        {
            Destroy(tile);
        }
        activeTargetTiles.Clear(); // Clear the list after destroying
    }
}
