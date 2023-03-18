using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GridSystem gridSystem;
    public float moveInterval;
    public float speed;

    private int currentX, currentY;

    private void Start()
    {
        currentX = Random.Range(0, gridSystem.GridSizeX);
        currentY = Random.Range(0, gridSystem.GridSizeY);
        transform.position = gridSystem.GetWorldPosition(currentX, currentY);

        InvokeRepeating(nameof(Move), moveInterval, moveInterval);
    }

    private void Move()
    {
        int newX = currentX + Random.Range(-1, 2);
        int newY = currentY + Random.Range(-1, 2);

        if (newX < 0 || newX >= gridSystem.GridSizeX || newY < 0 || newY >= gridSystem.GridSizeY || gridSystem.IsCellOccupied(newX, newY))
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(gridSystem.GetWorldPosition(newX, newY), 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<ObstacleSpawner>() != null)
            {
                return;
            }
        }

        Vector3 targetPosition = gridSystem.GetWorldPosition(newX, newY);
        StartCoroutine(MoveObject(targetPosition));
        gridSystem.SetCellState(currentX, currentY, false);
        gridSystem.SetCellState(newX, newY, true);
        currentX = newX;
        currentY = newY;
    }

    private IEnumerator MoveObject(Vector3 targetPosition)
    {
        float moveTime = Vector3.Distance(transform.position, targetPosition) / speed;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
