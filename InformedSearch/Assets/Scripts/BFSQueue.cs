using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSQueue : Queue
{
    public override void Add(Vector2Int position)
    {
        frontier.Add(position);
    }
}
