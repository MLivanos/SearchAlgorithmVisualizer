using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveMazeGenerator : TerrainGenerator
{   
    private void Start()
    {
        Initialize();
    }
    protected override void Initialize()
    {
        base.Initialize();
        MakeMaze();
    }

    protected override void MakeMaze()
    {
        AddGridLines();
        bool[,] visited = new bool[(int)shape.x,(int)shape.y];
        Vector2[,] previous = new Vector2[(int)shape.x,(int)shape.y];
        int xBound = (int)shape.x-2;
        int zBound = (int)shape.y-2;
        Vector2 initialPosition = new Vector2(1,1);
        Vector2 currentPosition = new Vector2(1,1);
        List<Vector2> neighbors = GetNeighborsRecursiveMaze(currentPosition, visited);
        while (initialPosition != currentPosition || neighbors.Count > 0)
        {
            visited[(int)currentPosition.x,(int)currentPosition.y] = true;
            if (neighbors.Count > 0)
            {
                Vector2 nextPosition = neighbors[Random.Range (0,neighbors.Count)];
                Vector2 offset = (nextPosition - currentPosition)/2;
                terrain[(int)(currentPosition.x + offset.x),(int)(currentPosition.y + offset.y)] = 0;
                previous[(int)nextPosition.x,(int)nextPosition.y] = currentPosition;
                currentPosition = nextPosition;
            }
            else
            {
                currentPosition = previous[(int)currentPosition.x,(int)currentPosition.y];
            }
            neighbors = GetNeighborsRecursiveMaze(currentPosition, visited);
        }
        base.MakeMaze();
    }

    private List<Vector2> GetNeighborsRecursiveMaze(Vector2 currentPosition, bool[,] visited)
    {
        List<Vector2> neighbors = new List<Vector2>();
        if (currentPosition.x > 1 && !visited[(int)currentPosition.x-2, (int)currentPosition.y])
        {
            neighbors.Add(new Vector2(currentPosition.x-2, currentPosition.y));
        }
        if (currentPosition.x < shape.x-2 && !visited[(int)currentPosition.x+2, (int)currentPosition.y])
        {
            neighbors.Add(new Vector2(currentPosition.x+2, currentPosition.y));
        }
        if (currentPosition.y > 1 && !visited[(int)currentPosition.x, (int)currentPosition.y-2])
        {
            neighbors.Add(new Vector2(currentPosition.x, currentPosition.y-2));
        }
        if (currentPosition.y < shape.y-2 && !visited[(int)currentPosition.x, (int)currentPosition.y+2])
        {
            neighbors.Add(new Vector2(currentPosition.x, currentPosition.y+2));
        }
        return neighbors;
    }

    private void AddGridLines()
    {
        for (int i = 1; i < (int)shape.x; i++)
        {
            for (int j = 1; j < (int)shape.y / 2; j++)
            {
                terrain[i,j*2] = blockedValue;
            }
        }
        for (int j = 1; j < (int)shape.y; j++)
        {
            for (int i = 1; i < (int)shape.x / 2; i++)
            {
                terrain[i*2,j] = blockedValue;
            }
        }
    }

}
