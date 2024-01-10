using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DjikstrasQueue : Queue
{
    Dictionary<Vector2Int, float> objectiveCost = new Dictionary<Vector2Int, float>();

    protected override void Add(Vector2Int position)
    {
        objectiveCost[position] = objectiveCost[previous[position]] + terrain.GetCost(position, previous[position]);
        InsertionSort(position, objectiveCost[position], objectiveCost);
    }
}
