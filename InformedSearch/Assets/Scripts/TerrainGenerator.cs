using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 shape;
    [SerializeField] private int[,] terrain;
    [SerializeField] private GameObject[] terrainPrefabs;
    [SerializeField] private GameObject cameraObject;
    private GameObject[,] terrainObjects;
    private float cellSize = 1.0f;
    private int freeValue = 0;
    private int blockedValue = 1;
    
    private void Start()
    {
        terrain = new int[(int)shape.x,(int)shape.y];
        terrainObjects = new GameObject[(int)shape.x,(int)shape.y];
        AddOutline();
        MakeRecursiveMaze();
        PositionCamera();
        StartCoroutine(GenerateMap());
    }

    private void MakeRecursiveMaze()
    {
        AddGridLines();
        bool[,] visited = new bool[(int)shape.x,(int)shape.y];
        Vector2[,] previous = new Vector2[(int)shape.x,(int)shape.y];
        int xBound = (int)shape.x-2;
        int zBound = (int)shape.y-2;
        Vector2 initialPosition = new Vector2(1,1);
        Vector2 currentPosition = new Vector2(1,1);
        List<Vector2> neighbors = GetNeighborsRecursiveMaze(currentPosition, visited);
        while (initialPosition != currentPosition || neighbors.Count > 0)
        {
            visited[(int)currentPosition.x,(int)currentPosition.y] = true;
            if (neighbors.Count > 0)
            {
                Vector2 nextPosition = neighbors[Random.Range (0,neighbors.Count)];
                Vector2 offset = (nextPosition - currentPosition)/2;
                terrain[(int)(currentPosition.x + offset.x),(int)(currentPosition.y + offset.y)] = 0;
                previous[(int)nextPosition.x,(int)nextPosition.y] = currentPosition;
                currentPosition = nextPosition;
            }
            else
            {
                currentPosition = previous[(int)currentPosition.x,(int)currentPosition.y];
            }
            neighbors = GetNeighborsRecursiveMaze(currentPosition, visited);
        }
    }

    private IEnumerator GenerateMap()
    {
        for (int i=0; i<(int)shape.x; i++)
        {
            for (int j=0; j<(int)shape.y; j++)
            {
                GameObject newTile = Instantiate(terrainPrefabs[terrain[i,j]]);
                terrainObjects[i,j] = newTile;
                newTile.transform.position = new Vector3(i*cellSize, 0, j*cellSize);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private void PositionCamera(float offset=1.2f)
    {
        cameraObject.transform.position = new Vector3(shape.x*cellSize / 2, Mathf.Max(shape.x*cellSize, shape.y*cellSize)*offset, shape.y*cellSize / 2);
    }

    private List<Vector2> GetNeighborsRecursiveMaze(Vector2 currentPosition, bool[,] visited)
    {
        List<Vector2> neighbors = new List<Vector2>();
        if (currentPosition.x > 1 && !visited[(int)currentPosition.x-2, (int)currentPosition.y])
        {
            neighbors.Add(new Vector2(currentPosition.x-2, currentPosition.y));
        }
        if (currentPosition.x < shape.x-2 && !visited[(int)currentPosition.x+2, (int)currentPosition.y])
        {
            neighbors.Add(new Vector2(currentPosition.x+2, currentPosition.y));
        }
        if (currentPosition.y > 1 && !visited[(int)currentPosition.x, (int)currentPosition.y-2])
        {
            neighbors.Add(new Vector2(currentPosition.x, currentPosition.y-2));
        }
        if (currentPosition.y < shape.y-2 && !visited[(int)currentPosition.x, (int)currentPosition.y+2])
        {
            neighbors.Add(new Vector2(currentPosition.x, currentPosition.y+2));
        }
        return neighbors;
    }

    private void AddGridLines()
    {
        for (int i = 1; i < (int)shape.x; i++)
        {
            for (int j = 1; j < (int)shape.y / 2; j++)
            {
                terrain[i,j*2] = blockedValue;
            }
        }
        for (int j = 1; j < (int)shape.y; j++)
        {
            for (int i = 1; i < (int)shape.x / 2; i++)
            {
                terrain[i*2,j] = blockedValue;
            }
        }
    }

    private void AddOutline()
    {
        for (int i = 0; i < shape.x; i++)
        {
            terrain[i,0] = blockedValue;
            terrain[i,(int)shape.y-1] = blockedValue;
        }
        for (int j=0; j < shape.y; j++)
        {
            terrain[0,j] = blockedValue;
            terrain[(int)shape.x-1,j] = blockedValue;
        }
    }

}
