using System.Collections;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    public GridSystem gridSystem;

    private int currentX, currentY;

    private void Start()
    {
        currentX = Random.Range(0, gridSystem.GridSizeX);
        currentY = Random.Range(0, gridSystem.GridSizeY);
        transform.position = gridSystem.GetWorldPosition(currentX, currentY);
    }

    private void OnTriggerEnter(Collider other)
    {
        /* if player script collide with skill
        if (other.GetComponent<PlayerController>() != null)
        {
            gridSystem.SetCellState(currentX, currentY, false);
            Destroy(gameObject);
        } */
    }
}
