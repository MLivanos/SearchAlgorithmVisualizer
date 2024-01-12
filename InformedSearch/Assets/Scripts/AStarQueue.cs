using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarQueue : Queue
{
    delegate float heuristicFunction(Vector2Int position);
    private Dictionary<Vector2Int, float> objectiveCost = new Dictionary<Vector2Int, float>();
    private Dictionary<Vector2Int, float> finalCost = new Dictionary<Vector2Int, float>();
    private heuristicFunction[] heuristics = new heuristicFunction[4];
    private int hueristicIndex;
    private float hueristicWeight = 1.0f;

    public override void Add(Vector2Int position)
    {
        objectiveCost[position] = objectiveCost[previous[position]] + terrain.GetCost(position, previous[position]);
        finalCost[position] = objectiveCost[position] + hueristicWeight*Hueristic(position);
        InsertionSort(position, finalCost[position], finalCost);
    }

    public override void Initialize()
    {
        objectiveCost[terrain.GetStart()] = 0.0f;
        previous[terrain.GetStart()] = terrain.GetStart();
        heuristics[0] = position => Zero(position);
        heuristics[1] = position => ManhattanDistance(position);
        heuristics[2] = position => ChebychevDistance(position);
        heuristics[3] = position => EuclideanDistance(position);
        base.Initialize();
    }

    protected float Hueristic(Vector2Int position)
    {
        return heuristics[hueristicIndex](position);
    }

    protected float ManhattanDistance(Vector2Int position)
    {
        return Mathf.Abs(position.x - goalPosition.x) + Mathf.Abs(position.y - goalPosition.y);
    }

    protected float ChebychevDistance(Vector2Int position)
    {
        return Mathf.Min(Mathf.Abs(position.x - goalPosition.x), Mathf.Abs(position.y - goalPosition.y));
    }

    protected float EuclideanDistance(Vector2Int position)
    {
        return Vector2.Distance(position, goalPosition);
    }

    protected float Zero(Vector2Int position)
    {
        return 0.0f;
    }

    public void SetHueristicIndex(int index)
    {
        hueristicIndex = index;
    }

    public void SetHueristicWeight(float weight)
    {
        hueristicWeight = weight;
    }
}
