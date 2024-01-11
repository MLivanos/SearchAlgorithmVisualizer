using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    private Color exploredColor = new Color32(0xDD, 0x44, 0x70, 0xFF);
    private Color pathColor = new Color32(0xFF, 0xC8, 0x72, 0xFF);
    private float timeBetweenExpansion;
    private Queue queue;
    private TerrainGenerator terrain;
    private HashSet<Vector2Int> explored;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        explored = new HashSet<Vector2Int>();
    }

    public IEnumerator Solve()
    {
        queue.EmptyFrontier();
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
}
