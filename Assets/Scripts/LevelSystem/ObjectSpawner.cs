using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GridSystem gridSystem;

    public GameObject[] objectPrefabs;
    public float objectSpawnInterval;
    public int maxObjects;

    public GameObject[] skillPrefabs;
    public float skillSpawnInterval;
    public int maxSkills;

    private int objectsSpawned;
    private int skillsSpawned;

    private void Start()
    {
        objectsSpawned = 0;
        StartCoroutine(SpawnObjects());

        skillsSpawned = 0;
        StartCoroutine(SpawnSkills());
    }

    private IEnumerator SpawnObjects()
    {
        while (objectsSpawned < maxObjects)
        {
            int randomX = Random.Range(0, gridSystem.GridSizeX);
            int randomY = Random.Range(0, gridSystem.GridSizeY);
            int randomPrefabIndex = Random.Range(0, objectPrefabs.Length);

            if (!gridSystem.IsCellOccupied(randomX, randomY))
            {
                Vector3 spawnPosition = gridSystem.GetWorldPosition(randomX, randomY);
                GameObject spawnedObject = Instantiate(objectPrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
                gridSystem.SetCellState(randomX, randomY, true);
                objectsSpawned++;

            }

            yield return new WaitForSeconds(objectSpawnInterval);
        }
    }

    private IEnumerator SpawnSkills()
    {
        while (skillsSpawned < maxSkills)
        {
            int randomX = Random.Range(0, gridSystem.GridSizeX);
            int randomY = Random.Range(0, gridSystem.GridSizeY);
            int randomPrefabIndex = Random.Range(0, skillPrefabs.Length);

            if (!gridSystem.IsCellOccupied(randomX, randomY))
            {
                Vector3 spawnPosition = gridSystem.GetWorldPosition(randomX, randomY);
                GameObject spawnedSkill = Instantiate(skillPrefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
                gridSystem.SetCellState(randomX, randomY, true);
                skillsSpawned++;
            }

            yield return new WaitForSeconds(skillSpawnInterval);
        }
    }
}
