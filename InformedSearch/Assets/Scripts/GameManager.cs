using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] terrainPrefabs;
    [SerializeField] private GameObject[] frontierPrefabs;
    [SerializeField] private TMP_Text nodesExploredDisplay;
    [SerializeField] private TMP_Text pathCostDisplay;
    [SerializeField] private float timeBetweenExpansion;
    [SerializeField] private int terrainIndex;
    [SerializeField] private int frontierIndex;
    private HashSet<Vector2> hasClicked = new HashSet<Vector2>();
    private Queue frontier;
    private TerrainGenerator terrain;
    private Solver solver;
    private float minSpeed = 0.3f;
    private bool isSolving;
    private bool isPlacingStart;
    private bool isPlacingGoal;
    private int randomMazeIndex;
    private int heuristicIndex;
    private float AStarWeight = 1.0f;
    private float maxAStarWeight = 2.0f;

    private void Start()
    {
        FindRandomMaze();
        InstantiateTerrain();
        InitializeFrontier();
    }

    private void Update()
    {
        CheckForClicks();
    }

    private void CheckForClicks()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, Camera.main.transform.position.y * Camera.main.transform.position.y))
            {
                int xPosition = (int)raycastHit.transform.position.x;
                int zPosition = (int)raycastHit.transform.position.z;
                Vector2Int clickedPosition = new Vector2Int(xPosition, zPosition);
                if (Input.GetMouseButtonDown(0) && Input.GetKey("s") && clickedPosition != terrain.GetGoal() && terrain.GetTile(clickedPosition) == 0.0f)
                {
                    terrain.SetStartPosition(clickedPosition);
                    ClearPath();
                    return;
                }
                if (Input.GetMouseButtonDown(0) && Input.GetKey("g")&& clickedPosition != terrain.GetStart() && terrain.GetTile(clickedPosition) == 0.0f)
                {
                    terrain.SetGoalPosition(clickedPosition);
                    ClearPath();
                    return;
                }
                if (hasClicked.Contains(clickedPosition))
                {
                    return;
                }
                hasClicked.Add(clickedPosition);
                terrain.SwapTile(clickedPosition);
            }
        }
        else
        {
            hasClicked = new HashSet<Vector2>();
        }
    }

    private void FindRandomMaze()
    {
        for(int i=0; i < terrainPrefabs.Length; i++)
        {
            GameObject prefab = terrainPrefabs[i];
            if (prefab.GetComponent<RandomMaze>())
            {
                randomMazeIndex = i;
                return;
            }
        }
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
        if (frontier is AStarQueue && !(frontier is DjikstrasQueue))
        {
            (frontier as AStarQueue).SetHueristicIndex(heuristicIndex);
            (frontier as AStarQueue).SetHueristicWeight(AStarWeight);
        }
        solver = gameObject.AddComponent(typeof(Solver)) as Solver;
        solver.SetText(nodesExploredDisplay, pathCostDisplay);
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
        solver.Solve();
    }

    public void ClearPath()
    {
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

    public void Refresh()
    {
        ChangeMaze(terrainIndex);
    }

    public void ChangeObstacleProportion(float proportion)
    {
        terrainPrefabs[randomMazeIndex].GetComponent<RandomMaze>().SetProportion(proportion);
    }

    public void SetAStarHueristic(int index)
    {
        heuristicIndex = index;
        InitializeFrontier();
    }

    public void SetAStarWeight(float weight)
    {
        AStarWeight = maxAStarWeight * weight;
        InitializeFrontier();
    }
}
