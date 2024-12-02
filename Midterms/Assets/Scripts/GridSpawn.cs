using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawn : MonoBehaviour
{
    [SerializeField] private int _size;
    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(i, j), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";
                
                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                spawnedTile.Init(isOffset);
            }
        }
        
        _cam.transform.position = new Vector3((float)_size/2 - 0.5f, (float)_size/2 - 0.5f, -10);
    }
}
