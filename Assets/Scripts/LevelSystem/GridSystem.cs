using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_Plane;
    [SerializeField] private float m_CellSize;

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
        Vector3 planeSize = m_Plane.GetComponent<Renderer>().bounds.size;
        gridSizeX = Mathf.FloorToInt(planeSize.x / m_CellSize);
        gridSizeY = Mathf.FloorToInt(planeSize.z / m_CellSize);
        origin = m_Plane.transform.position - planeSize / 2 + new Vector3(m_CellSize / 2, 0, m_CellSize / 2);
    }

    private void InitializeGrid()
    {
        grid = new bool[gridSizeX, gridSizeY];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * m_CellSize, 0, y * m_CellSize) + origin + new Vector3(0, 0.5f, 0); ;
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
