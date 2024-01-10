using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2 shape;
    [SerializeField] protected int[,] terrain;
    [SerializeField] protected GameObject[] terrainPrefabs;
    [SerializeField] protected GameObject cameraObject;
    [SerializeField] protected float mazeInitializationTime;
    protected GameObject[,] terrainObjects;
    protected float cellSize = 1.0f;
    protected int freeValue = 0;
    protected int blockedValue = 1;
    
    protected void Initialize()
    {
        terrain = new int[(int)shape.x,(int)shape.y];
        terrainObjects = new GameObject[(int)shape.x,(int)shape.y];
        AddOutline();
        PositionCamera();
        StartCoroutine(GenerateMap());
    }

    protected IEnumerator GenerateMap()
    {
        float timeBetweenBlocks = mazeInitializationTime / (shape.x*shape.y);
        for (int i=0; i<(int)shape.x; i++)
        {
            for (int j=0; j<(int)shape.y; j++)
            {
                GameObject newTile = Instantiate(terrainPrefabs[terrain[i,j]]);
                terrainObjects[i,j] = newTile;
                newTile.transform.position = new Vector3(i*cellSize, 0, j*cellSize);
                yield return new WaitForSeconds(timeBetweenBlocks);
            }
        }
    }

    protected void PositionCamera(float offset=1.2f)
    {
        cameraObject.transform.position = new Vector3(shape.x*cellSize / 2, Mathf.Max(shape.x*cellSize, shape.y*cellSize)*offset, shape.y*cellSize / 2);
    }

    protected void AddOutline()
    {
        for (int i = 0; i < shape.x; i++)
        {
            terrain[i,0] = blockedValue;
            terrain[i,(int)shape.y-1] = blockedValue;
        }
        for (int j=0; j < shape.y; j++)
        {
            terrain[0,j] = blockedValue;
            terrain[(int)shape.x-1,j] = blockedValue;
        }
    }

}
