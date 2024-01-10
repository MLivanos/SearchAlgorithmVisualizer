using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarQueue : Queue
{
    Dictionary<Vector2Int, float> objectiveCost = new Dictionary<Vector2Int, float>();
    Dictionary<Vector2Int, float> finalCost = new Dictionary<Vector2Int, float>();

    public override void Add(Vector2Int position)
    {
        objectiveCost[position] = objectiveCost[previous[position]] + terrain.GetCost(position, previous[position]);
        finalCost[position] = objectiveCost[position] + Hueristic(position);
        InsertionSort(position, finalCost[position], finalCost);
    }

    public override void Initialize()
    {
        objectiveCost[terrain.GetStart()] = 0.0f;
        previous[terrain.GetStart()] = terrain.GetStart();
        base.Initialize();
    }

    private float Hueristic(Vector2Int position)
    {
        return Vector2.Distance(position, goalPosition);
    }
}
