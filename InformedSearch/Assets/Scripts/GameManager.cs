using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] terrainPrefabs;
    [SerializeField] private GameObject[] frontierPrefabs;
    [SerializeField] private float timeBetweenExpansion;
    [SerializeField] private int terrainIndex;
    [SerializeField] private int frontierIndex;
    private Queue frontier;
    private TerrainGenerator terrain;
    private Solver solver;
    private bool isSolving;

    private void Start()
    {
        GameObject terrainObject = Instantiate(terrainPrefabs[terrainIndex]);
        terrain = terrainObject.GetComponent<TerrainGenerator>();
        InitializeFrontier();
    }

    private void InitializeFrontier()
    {
        GameObject frontierObject = Instantiate(frontierPrefabs[frontierIndex]);
        frontier = frontierObject.GetComponent<Queue>();
        if (solver)
        {
            Destroy(GetComponent<Solver>());
        }
        solver = gameObject.AddComponent(typeof(Solver)) as Solver;
        solver.SetTerrain(terrain);
        solver.SetQueue(frontier);
        frontier.Initialize();
        solver.SetWaitTime(timeBetweenExpansion);
    }

    private void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            ChangeAlgorithm(0);
        }
        if (Input.GetKeyDown("1"))
        {
            ChangeAlgorithm(1);
        }
        if (Input.GetKeyDown("2"))
        {
            ChangeAlgorithm(2);
        }
        if (Input.GetKeyDown("3"))
        {
            ChangeAlgorithm(3);
        }
        if (Input.GetKeyDown("r"))
        {
            isSolving = false;
            solver.Initialize();
            terrain.ResetMaze();
        }
        if (Input.GetKeyDown("s") && terrain.IsCreated() && !isSolving)
        {
            isSolving = true;
            StartCoroutine(solver.Solve());
        }
    }

    public void ChangeAlgorithm(int index)
    {
        frontierIndex = index;
        InitializeFrontier();
    }
}
