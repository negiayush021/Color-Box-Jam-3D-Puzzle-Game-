using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 3;
    public int columns = 3;

    public Vector3 origin = new Vector3(-3, 0, -3);
    public GameObject tilePrefab;

    public Vector2[] dont_spawn_coordinate;

    public float spacing = 1.5f;

    private void Start()
    {
        CreateGrid();
    }


    void CreateGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (dont_spawn_coordinate != null)
                {
                    // Check if (i,j) is in dont_spawn_coordinate
                    bool shouldSkip = false;
                    foreach (var coord in dont_spawn_coordinate)
                    {
                        if (coord.x == i && coord.y == j)
                        {
                            shouldSkip = true;
                            break;
                        }
                    }

                    if (shouldSkip)
                    {
                        continue; // skip spawning here
                    }


                    Vector3 spawnPos = origin + new Vector3(i * spacing, transform.position.y, j * spacing);
                    Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                }

                
                
                
            }
        }
    }
}
