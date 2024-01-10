using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject terrainPrefab;
    [SerializeField] private Queue queue;
    private TerrainGenerator terrain;
    private Solver solver;
    private bool isSolving;

    private void Start()
    {
        solver = gameObject.AddComponent(typeof(Solver)) as Solver;
        queue = gameObject.AddComponent(typeof(BFSQueue)) as Queue;
        GameObject terrainObject = Instantiate(terrainPrefab);
        terrain = terrainObject.GetComponent<TerrainGenerator>();
        solver.SetTerrain(terrain);
        solver.SetQueue(queue);
    }

    private void Update()
    {
        if (terrain.IsCreated() && !isSolving)
        {
            isSolving = true;
            StartCoroutine(solver.Solve());
        }
    }
}
