using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaze : TerrainGenerator
{   
    [SerializeField] private float proportionOfBlockedPoints;

    public override void Initialize(Vector2Int mazeShape)
    {
        base.Initialize(mazeShape);
        MakeMaze();
    }

    protected override void MakeMaze()
    {
        for(int i=0; i<(int)(proportionOfBlockedPoints*shape.x*shape.y); i++)
        {
            int randomX = Random.Range(0,(int)shape.x);
            int randomZ = Random.Range(0,(int)shape.y);
            terrain[randomX,randomZ] = blockedValue;
        }
        base.MakeMaze();
    }

    public void SetBlockedProportion(float proportion)
    {
        proportionOfBlockedPoints = proportion;
    }

    public void SetProportion(float proportion)
    {
        proportionOfBlockedPoints = proportion;
    }
}
