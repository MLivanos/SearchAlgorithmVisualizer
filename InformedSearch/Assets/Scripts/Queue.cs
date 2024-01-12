using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Queue : MonoBehaviour
{
    protected TerrainGenerator terrain;
    protected List<Vector2Int> frontier;
    protected Vector2Int goalPosition;
    protected Dictionary<Vector2Int, Vector2Int> previous = new Dictionary<Vector2Int, Vector2Int>();

    private void Start()
    {
        Initialize();
    }

    public virtual void Add(Vector2Int position)
    {

    }

    public virtual void Initialize()
    {
        goalPosition = terrain.GetGoal();
        frontier = new List<Vector2Int>();
    }

    public Vector2Int Pop()
    {
        if (frontier.Count == 0)
        {
            return new Vector2Int(-1,-1);
        }
        Vector2Int recentItem = frontier[0];
        frontier.RemoveAt(0);
        return recentItem;
    }

    protected void InsertionSort(Vector2Int position, float cost, Dictionary<Vector2Int, float> positionToCost)
    {
        int low = 0;
        int high = frontier.Count - 1;
        int middle = low + (int)((high-low)/2);
        while (low < high)
        {
            float middleValue = positionToCost[frontier[middle]];
            if (middleValue == cost)
            {
                break;
            }
            if (middleValue < cost)
            {
                low = middle + 1;
            }
            else
            {
                high = middle - 1;
            }
            middle = low + (int)((high-low)/2);
        }
        frontier.Insert(middle, position);
    }

    public void UpdateGoalPosition(Vector2Int goal)
    {
        goalPosition = goal;
    }

    public void SetFrontier(TerrainGenerator terrain_)
    {
        terrain = terrain_;
    }

    public void AddToHistory(Vector2Int node, Vector2Int successor)
    {
        previous[successor] = node;
    }

    public bool IsEmpty()
    {
        return frontier.Count == 0;
    }

    public void SetTerrain(TerrainGenerator terrain_)
    {
        terrain = terrain_;
    }

    public List<Vector2Int> Backtrack()
    {
        List<Vector2Int> pathway = new List<Vector2Int>();
        Vector2Int currentPoint = terrain.GetGoal();
        while (currentPoint != terrain.GetStart())
        {
            pathway.Add(currentPoint);
            currentPoint = previous[currentPoint];
        }
        pathway.Add(terrain.GetStart());
        pathway.Reverse();
        return pathway;
    }

    public void PrintFrontier()
    {
        Debug.Log("============");
        for(int i=0; i<frontier.Count; i++)
        {
            Debug.Log(frontier[i]);
        }
        Debug.Log("============");
    }

    public void EmptyFrontier()
    {
        frontier = new List<Vector2Int>();
    }

}
