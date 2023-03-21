using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    private void Start()
    {
        GridSystem gridSystem = GameManager.Instance.LevelManager.GridSystem;
    }
}
