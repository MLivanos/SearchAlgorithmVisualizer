using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    private Color exploredColor = Color.yellow;
    private Color pathColor = Color.green;
    private float timeBetweenExpansion;
    private Queue queue;
    private TerrainGenerator terrain;
    private HashSet<Vector2Int> explored;

    private void Start()
    {
        explored = new HashSet<Vector2Int>();
    }

    public IEnumerator Solve()
    {
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
    }

    public void HighlightPath(List<Vector2Int> path)
    {
        foreach(Vector2Int point in path)
        {
            terrain.ExploreNode(point, pathColor);
        }
    }

    private void AddNeighbor(Vector2Int currentPosition, Vector2Int neighbor)
    {
        if (explored.Contains(neighbor))
        {
            return;
        }
        queue.Add(neighbor);
        explored.Add(neighbor);
        queue.AddToHistory(currentPosition, neighbor);
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
}
