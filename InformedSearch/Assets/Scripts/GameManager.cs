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
    private float minSpeed = 0.3f;
    private bool isSolving;

    private void Start()
    {
        InstantiateTerrain();
        InitializeFrontier();
    }

    private void InstantiateTerrain()
    {
        if (terrain)
        {
            terrain.DestroyMaze();
        }
        GameObject terrainObject = Instantiate(terrainPrefabs[terrainIndex]);
        terrain = terrainObject.GetComponent<TerrainGenerator>();
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

    public void ChangeAlgorithm(int index)
    {
        frontierIndex = index;
        InitializeFrontier();
    }

    public void ChangeMaze(int index)
    {
        terrainIndex = index;
        InstantiateTerrain();
        InitializeFrontier();
    }

    public void StartSimulation()
    {
        isSolving = true;
        StartCoroutine(solver.Solve());
    }

    public void ClearPath()
    {
        isSolving = false;
        solver.Initialize();
        terrain.ResetMaze();
    }

    public void ChangeSimulationSpeed(float speed)
    {
        if(!solver)
        {
            return;
        }
        solver.SetWaitTime(minSpeed * (1.0f-speed));
    }
}
