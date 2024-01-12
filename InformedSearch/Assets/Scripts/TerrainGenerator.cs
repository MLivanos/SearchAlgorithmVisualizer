using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int shape;
    [SerializeField] protected Vector2Int startPoint;
    [SerializeField] protected Vector2Int goalPoint;
    [SerializeField] protected GameObject[] terrainPrefabs;
    [SerializeField] protected float mazeInitializationTime;
    [SerializeField] protected float itemFlipTime;
    protected Color startColor = new Color32(0x4E, 0x4D, 0x8D, 0xFF);
    protected Color goalColor = new Color32(0x84, 0xD8, 0x8B, 0xFF);
    protected GameObject[,] terrainObjects;
    protected Color freeSpaceColor;
    protected int[,] terrain;
    protected GameObject cameraObject;
    protected float cellSize = 1.0f;
    protected int freeValue = 0;
    protected int blockedValue = 1;
    protected bool isCreated;

    public virtual void Initialize(Vector2Int mazeShape)
    {
        shape = mazeShape;
        Renderer tileRenderer = terrainPrefabs[freeValue].GetComponent<Renderer>();
        freeSpaceColor = tileRenderer.sharedMaterial.color;
        terrain = new int[shape.x,shape.y];
        terrainObjects = new GameObject[shape.x,shape.y];
        cameraObject = GameObject.Find("Main Camera");
        AddOutline();
        PositionCamera();
    }

    protected virtual void MakeMaze()
    {
        StartCoroutine(GenerateMap());
    }

    public void ExploreNode(Vector2Int position, Color color)
    {
        //StartCoroutine(FlipTile(position));
        StartCoroutine(ChangePlaceColor(position, color, itemFlipTime));
    }

    public IEnumerator FlipTile(Vector2Int position)
    {
        GameObject tile = terrainObjects[position.x, position.y];
        float timer = 0.0f;
        float rotationSpeed = 180.0f/itemFlipTime;
        while (timer < itemFlipTime)
        {
            timer += Time.deltaTime;
            tile.transform.Rotate(Vector3.forward*rotationSpeed*Time.deltaTime);
            yield return null;
        }
        tile.transform.eulerAngles = Vector3.zero;
    }

    private IEnumerator ChangePlaceColor(Vector2Int position, Color color, float time)
    {
        GameObject tile = terrainObjects[position.x, position.y];
        Renderer objectRenderer = tile.GetComponent<Renderer>();
        float timer = 0.0f;
        Color intermediateColor = objectRenderer.material.color;
        Color initialColor = objectRenderer.material.color;
        while (timer < time)
        {
            timer += Time.deltaTime;
            intermediateColor += Time.deltaTime/time * (color - initialColor);
            objectRenderer.material.SetColor("_BaseColor", intermediateColor);
            yield return null;
        }
        objectRenderer.material.SetColor("_BaseColor", color);
    }

    private IEnumerator GenerateMap()
    {
        float timeBetweenBlocks = mazeInitializationTime / shape.y;
        for (int i=0; i<shape.x; i++)
        {
            for (int j=0; j<shape.y; j++)
            {
                GameObject newTile = Instantiate(terrainPrefabs[terrain[i,j]]);
                terrainObjects[i,j] = newTile;
                newTile.transform.position = new Vector3(i*cellSize, 0, j*cellSize);
            }
            yield return new WaitForSeconds(timeBetweenBlocks);
        }
        SetStartPosition(new Vector2Int((int)(shape.x * 0.25),(int)(shape.y * 0.5)));
        SetGoalPosition(new Vector2Int((int)(shape.x * 0.75),(int)(shape.y * 0.5)));
        StartCoroutine(ChangePlaceColor(new Vector2Int(startPoint.x, startPoint.y), startColor, 0.0f));
        StartCoroutine(ChangePlaceColor(new Vector2Int(goalPoint.x, goalPoint.y), goalColor, 0.0f));
        isCreated = true;
    }

    protected void PositionCamera(float offset=1.33f)
    {
        cameraObject.transform.position = new Vector3((shape.x*cellSize - 1) / 2, Mathf.Min(shape.x*cellSize, shape.y*cellSize)*offset, shape.y*cellSize / 2 - 1);
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

    public void SwapTile(Vector2Int position)
    {
        if (IsRegularTile(position))
        {
            return;
        }
        terrain[position.x, position.y] = 1 - terrain[position.x, position.y];
        GameObject newTile = Instantiate(terrainPrefabs[terrain[position.x, position.y]]);
        newTile.transform.position = terrainObjects[position.x, position.y].transform.position;
        Destroy(terrainObjects[position.x, position.y]);
        terrainObjects[position.x, position.y] = newTile;
    }

    private bool IsRegularTile(Vector2Int position)
    {
        return IsInBounds(position) || position == GetStart() || position == GetGoal();
    }

    private bool IsInBounds(Vector2Int position)
    {
        return position.x >= shape.x - 1 || position.y >= shape.y - 1 || position.x * position.y <= 0;
    }

    public int GetCost(Vector2Int position1, Vector2Int position2)
    {
        if (Mathf.Abs((position2 - position1).magnitude) > 1.1f)
        {
            return 100000000;
        }
        return (int)Mathf.Abs((position2 - position1).magnitude);
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

    public void SetCamera(GameObject camera_)
    {
        cameraObject = camera_;
    }

    public void SetShape(Vector2Int shape_)
    {
        shape = shape_;
    }

    public void SetStartPosition(Vector2Int position)
    {
        if (IsRegularTile(position))
        {
            return;
        }
        if (GetTile(position) == blockedValue)
        {
            SwapTile(position);
        }
        startPoint = position;
    }

    public void SetGoalPosition(Vector2Int position)
    {
        if (IsRegularTile(position))
        {
            return;
        }
        if (GetTile(position) == blockedValue)
        {
            SwapTile(position);
        }
        goalPoint = position;
    }

    public void SetInitializationTime(float time)
    {
        mazeInitializationTime = time;
    }

    public void SetItemFlipTime(float time)
    {
        itemFlipTime = time;
    }

    public Vector2Int GetStart()
    {
        return startPoint;
    }

    public Vector2Int GetGoal()
    {
        return goalPoint;
    }

    public bool IsCreated()
    {
        return isCreated;
    }

    public void ResetMaze()
    {
        for(int i=0; i<shape.x; i++)
        {
            for(int j=0; j<shape.y; j++)
            {
                if (terrain[i,j] == freeValue)
                {
                    StartCoroutine(ChangePlaceColor(new Vector2Int(i,j), freeSpaceColor, 0.0f));
                }
            }
        }
        StartCoroutine(ChangePlaceColor(startPoint, startColor, 0.0f));
        StartCoroutine(ChangePlaceColor(goalPoint, goalColor, 0.0f));
    }

    public void DestroyMaze()
    {
        for(int i=0; i<shape.x; i++)
        {
            for(int j=0; j<shape.y; j++)
            {
                Destroy(terrainObjects[i,j]);
            }
        }
        Destroy(gameObject);
    }
}
