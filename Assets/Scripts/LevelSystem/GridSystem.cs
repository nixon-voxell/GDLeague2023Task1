using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_Plane;
    [SerializeField] private float m_CellSize;

    private int m_GridSizeX, m_GridSizeY;
    private Vector3 m_Origin;
    private bool[,] m_Grid;

    public int gridSizeX => m_GridSizeX;
    public int gridSizeY => m_GridSizeY;

    private void Start()
    {
        CalculateGridSize();
        InitializeGrid();
    }

    private void CalculateGridSize()
    {
        Vector3 planeSize = m_Plane.GetComponent<Renderer>().bounds.size;
        m_GridSizeX = Mathf.FloorToInt(planeSize.x / m_CellSize);
        m_GridSizeY = Mathf.FloorToInt(planeSize.z / m_CellSize);
        m_Origin = m_Plane.transform.position - planeSize / 2 + new Vector3(m_CellSize / 2, 0, m_CellSize / 2);

        Debug.Log(m_GridSizeX);
        Debug.Log(m_GridSizeY);
    }

    private void InitializeGrid()
    {
        m_Grid = new bool[m_GridSizeX, m_GridSizeY];
    }

    public bool IsCellOccupied(int x, int y)
    {
        if (x >= 0 && x < m_GridSizeX && y >= 0 && y < m_GridSizeY)
        {
            return m_Grid[x, y];
        }
        return true;
    }

    public void SetCellState(int x, int y, bool isOccupied)
    {
        if (x >= 0 && x < m_GridSizeX && y >= 0 && y < m_GridSizeY)
        {
            m_Grid[x, y] = isOccupied;
        }
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - m_Origin.x) / m_CellSize);
        int y = Mathf.FloorToInt((worldPosition.z - m_Origin.z) / m_CellSize);

        return new Vector2Int(x, y);
    }
}
