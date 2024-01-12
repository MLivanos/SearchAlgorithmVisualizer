using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Solver : MonoBehaviour
{
    private Color exploredColor = new Color32(0xDD, 0x44, 0x70, 0xFF);
    private Color pathColor = new Color32(0xFF, 0xC8, 0x72, 0xFF);
    private Coroutine lastRoutine = null;
    private float timeBetweenExpansion;
    private Queue queue;
    private TerrainGenerator terrain;
    private HashSet<Vector2Int> explored;
    private TMP_Text nodesExploredDisplay;
    private TMP_Text pathCostDisplay;
    public int pathCost;


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (lastRoutine != null)
        {
            StopCoroutine(lastRoutine);
        }
        explored = new HashSet<Vector2Int>();
        ChangeDisplay();
    }

    public void Solve()
    {
        lastRoutine = StartCoroutine(SolveMaze());
    }

    public IEnumerator SolveMaze()
    {
        queue.Initialize();
        queue.Add(terrain.GetStart());
        while (!queue.IsEmpty())
        {
            Vector2Int currentPosition = queue.Pop();
            if (currentPosition == terrain.GetGoal())
            {
                HighlightPath(queue.Backtrack());
                break;
            }
            terrain.ExploreNode(currentPosition, exploredColor);
            List<Vector2Int> neighbors = terrain.GetNeighbors(currentPosition);
            foreach(Vector2Int neighbor in neighbors)
            {
                AddNeighbor(currentPosition, neighbor);
            }
            yield return new WaitForSeconds(timeBetweenExpansion);
        }
        ChangeDisplay();
    }

    public void HighlightPath(List<Vector2Int> path)
    {
        pathCost = 0;
        foreach(Vector2Int point in path)
        {
            pathCost += 1;
            terrain.ExploreNode(point, pathColor);
        }
    }

    private void AddNeighbor(Vector2Int currentPosition, Vector2Int neighbor)
    {
        if (explored.Contains(neighbor))
        {
            return;
        }
        queue.AddToHistory(currentPosition, neighbor);
        queue.Add(neighbor);
        explored.Add(neighbor);
    }

    public void SetQueue(Queue queue_)
    {
        queue = queue_;
        queue.SetTerrain(terrain);
        queue.UpdateGoalPosition(terrain.GetGoal());
    }

    public void SetTerrain(TerrainGenerator terrain_)
    {
        terrain = terrain_;
    }

    public void SetWaitTime(float time)
    {
        timeBetweenExpansion = time;
    }

    public float GetPathCost()
    {
        return pathCost;
    }

    public int GetNodesExplored()
    {
        return explored.Count;
    }

    public void SetText(TMP_Text nodesExploredText, TMP_Text pathCostText)
    {
        nodesExploredDisplay = nodesExploredText;
        pathCostDisplay = pathCostText;
    }

    private void ChangeDisplay()
    {
        nodesExploredDisplay.text = GetNodesExplored().ToString();
        pathCostDisplay.text = pathCost.ToString();
    }
}
