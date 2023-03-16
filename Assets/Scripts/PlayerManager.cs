using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject m_PlayerPrefab;

    private void Start()
    {
        GameManager.Instance.PlayerManager = this;
    }

    public void MovePlayerToScene(GameObject playerGO)
    {
        SceneManager.MoveGameObjectToScene(playerGO, this.gameObject.scene);
    }
}
