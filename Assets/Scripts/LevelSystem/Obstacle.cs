using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GridSystem m_GridSystem;

    private int m_CellX, m_CellY;

    private void Start()
    {
        Vector2Int gridPosition = m_GridSystem.WorldToGridPosition(transform.position);
        m_GridSystem.SetCellState(gridPosition.x, gridPosition.y, true);

        Debug.Log("Cell X: " + gridPosition.x);
        Debug.Log("Cell Y: " + gridPosition.y);
    }
}
