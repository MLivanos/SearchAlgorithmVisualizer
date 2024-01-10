using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2 shape;
    [SerializeField] protected int[,] terrain;
    [SerializeField] protected GameObject[] terrainPrefabs;
    [SerializeField] protected GameObject cameraObject;
    [SerializeField] protected float mazeInitializationTime;
    protected GameObject[,] terrainObjects;
    protected float cellSize = 1.0f;
    protected int freeValue = 0;
    protected int blockedValue = 1;
    
    protected virtual void Initialize()
    {
        terrain = new int[(int)shape.x,(int)shape.y];
        terrainObjects = new GameObject[(int)shape.x,(int)shape.y];
        AddOutline();
        PositionCamera();
    }

    protected virtual void MakeMaze()
    {
        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap()
    {
        float timeBetweenBlocks = mazeInitializationTime / (shape.x*shape.y);
        for (int i=0; i<(int)shape.x; i++)
        {
            for (int j=0; j<(int)shape.y; j++)
            {
                GameObject newTile = Instantiate(terrainPrefabs[terrain[i,j]]);
                terrainObjects[i,j] = newTile;
                newTile.transform.position = new Vector3(i*cellSize, 0, j*cellSize);
                yield return new WaitForSeconds(timeBetweenBlocks);
            }
        }
    }

    protected void PositionCamera(float offset=1.2f)
    {
        cameraObject.transform.position = new Vector3(shape.x*cellSize / 2, Mathf.Max(shape.x*cellSize, shape.y*cellSize)*offset, shape.y*cellSize / 2);
    }

    protected void AddOutline()
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

    public int GetTile(Vector2 position)
    {
        return terrain[(int)position.x, (int)position.y];
    }

    public List<Vector2> GetNeighbors(Vector2 position)
    {
        List<Vector2> neighbors = new List<Vector2>();
        if (canTravelTo(position + Vector2.left))
        {
            neighbors.Add(position + Vector2.left);
        }
        if (canTravelTo(position + Vector2.right))
        {
            neighbors.Add(position + Vector2.right);
        }
        if (canTravelTo(position + Vector2.up))
        {
            neighbors.Add(position + Vector2.up);
        }
        if (canTravelTo(position + Vector2.down))
        {
            neighbors.Add(position + Vector2.down);
        }
        return neighbors;
    }

    // Assumes that this is within 1 block of a known travelable position
    private bool canTravelTo(Vector2 position)
    {
        return GetTile(position) != blockedValue;
    }
}
