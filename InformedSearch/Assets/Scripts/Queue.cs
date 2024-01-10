using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Queue : MonoBehaviour
{
    protected TerrainGenerator terrain;
    protected List<Vector2Int> frontier;
    protected Vector2 goalPosition;
    protected Dictionary<Vector2Int, Vector2Int> previous = new Dictionary<Vector2Int, Vector2Int>();

    protected virtual void Add(Vector2Int position)
    {

    }

    protected Vector2 Pop()
    {
        if (frontier.Count == 0)
        {
            return new Vector2(-1,-1);
        }
        Vector2 recentItem = frontier[0];
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
            if (middleValue >= cost)
            {
                low += middle;
            }
            else
            {
                high -= middle;
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
}
