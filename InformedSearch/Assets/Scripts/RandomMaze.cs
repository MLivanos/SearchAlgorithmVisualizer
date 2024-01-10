using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaze : TerrainGenerator
{   
    [SerializeField] private float proportionOfBlockedPoints;
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
        for(int i=0; i<(int)(proportionOfBlockedPoints*shape.x*shape.y); i++)
        {
            int randomX = Random.Range(0,(int)shape.x);
            int randomZ = Random.Range(0,(int)shape.y);
            terrain[randomX,randomZ] = blockedValue;
        }
        base.MakeMaze();
    }

}
