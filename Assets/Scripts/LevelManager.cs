using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GridSystem m_GridSystem;

    public GridSystem GridSystem => this.m_GridSystem;

    private void Start()
    {
        GameManager.Instance.LevelManager = this;
    }

    public void MovePlayerToScene(GameObject playerGO)
    {
        SceneManager.MoveGameObjectToScene(playerGO, this.gameObject.scene);
    }
}
