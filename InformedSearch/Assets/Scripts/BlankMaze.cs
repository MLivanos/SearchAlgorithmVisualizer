using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankMaze : TerrainGenerator
{ 

    public override void Initialize(Vector2Int mazeShape)
    {
        base.Initialize(mazeShape);
        MakeMaze();
    }
}
