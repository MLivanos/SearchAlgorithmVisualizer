using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int shape;
    [SerializeField] protected Vector2Int startPoint;
    [SerializeField] protected Vector2Int goalPoint;
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
        terrain = new int[shape.x,shape.y];
        terrainObjects = new GameObject[shape.x,shape.y];
        AddOutline();
        PositionCamera();
    }

    protected virtual void MakeMaze()
    {
        terrain[startPoint.x, startPoint.y] = freeValue;
        terrain[goalPoint.x, goalPoint.y] = freeValue;
        StartCoroutine(GenerateMap());
    }

    private void ChangePlaceColor(GameObject placeObject, Color color)
    {
        Renderer objectRenderer = placeObject.GetComponent<Renderer>();
        objectRenderer.material.SetColor("_Color", color);
    }

    private IEnumerator GenerateMap()
    {
        float timeBetweenBlocks = mazeInitializationTime / (shape.x*shape.y);
        for (int i=0; i<shape.x; i++)
        {
            for (int j=0; j<shape.y; j++)
            {
                GameObject newTile = Instantiate(terrainPrefabs[terrain[i,j]]);
                terrainObjects[i,j] = newTile;
                newTile.transform.position = new Vector3(i*cellSize, 0, j*cellSize);
                yield return new WaitForSeconds(timeBetweenBlocks);
            }
        }
        ChangePlaceColor(terrainObjects[startPoint.x, startPoint.y], Color.red);
        ChangePlaceColor(terrainObjects[goalPoint.x, goalPoint.y], Color.blue);
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
            terrain[i,shape.y-1] = blockedValue;
        }
        for (int j=0; j < shape.y; j++)
        {
            terrain[0,j] = blockedValue;
            terrain[shape.x-1,j] = blockedValue;
        }
    }

    public int GetTile(Vector2Int position)
    {
        return terrain[position.x, position.y];
    }

    public List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (canTravelTo(position + Vector2Int.left))
        {
            neighbors.Add(position + Vector2Int.left);
        }
        if (canTravelTo(position + Vector2Int.right))
        {
            neighbors.Add(position + Vector2Int.right);
        }
        if (canTravelTo(position + Vector2Int.up))
        {
            neighbors.Add(position + Vector2Int.up);
        }
        if (canTravelTo(position + Vector2Int.down))
        {
            neighbors.Add(position + Vector2Int.down);
        }
        return neighbors;
    }

    // Assumes that this is within 1 block of a known travelable position
    private bool canTravelTo(Vector2Int position)
    {
        return GetTile(position) != blockedValue;
    }
}
