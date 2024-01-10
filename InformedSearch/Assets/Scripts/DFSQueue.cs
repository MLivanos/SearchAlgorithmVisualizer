using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSQueue : Queue
{
    public override void Add(Vector2Int position)
    {
        frontier.Insert(frontier.Count, position);
    }
}
