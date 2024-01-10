using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSQueue : Queue
{
    protected override void Add(Vector2Int position)
    {
        frontier.Add(position);
    }
}
