using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public GameObject plane;
    public float cellSize;

    private int gridSizeX, gridSizeY;
    private Vector3 origin;
    private bool[,] grid;

    public int GridSizeX => gridSizeX;
    public int GridSizeY => gridSizeY;

    private void Start()
    {
        CalculateGridSize();
        InitializeGrid();
    }

    private void CalculateGridSize()
    {
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;
        gridSizeX = Mathf.FloorToInt(planeSize.x / cellSize);
        gridSizeY = Mathf.FloorToInt(planeSize.z / cellSize);
        origin = plane.transform.position - planeSize / 2 + new Vector3(cellSize / 2, 0, cellSize / 2);
    }

    private void InitializeGrid()
    {
        grid = new bool[gridSizeX, gridSizeY];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, 0, y * cellSize) + origin + new Vector3(0, 0.5f, 0); ;
    }

    public bool IsCellOccupied(int x, int y)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return grid[x, y];
        }
        return true;
    }

    public void SetCellState(int x, int y, bool isOccupied)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            grid[x, y] = isOccupied;
        }
    }
}
