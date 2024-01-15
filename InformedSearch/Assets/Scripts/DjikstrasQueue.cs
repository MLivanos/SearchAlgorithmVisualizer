using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DjikstrasQueue : Queue
{
    Dictionary<Vector2Int, float> objectiveCost = new Dictionary<Vector2Int, float>();

    public override void Add(Vector2Int position)
    {
        objectiveCost[position] = objectiveCost[previous[position]] + terrain.GetCost(position, previous[position]);
        Insert(position, objectiveCost[position], objectiveCost);
    }

    public override void Initialize()
    {
        objectiveCost[terrain.GetStart()] = 0.0f;
        previous[terrain.GetStart()] = terrain.GetStart();
        base.Initialize();
    }
}
