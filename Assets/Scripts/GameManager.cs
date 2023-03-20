using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera m_Camera;
    [HideInInspector] public LevelManager LevelManager;

    [SerializeField, Voxell.Util.Scene] private string[] m_InitialScenes;

    public Camera MainCamera => this.m_Camera;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        // get all active scenes
        string[] loadedScenes = new string[SceneManager.sceneCount];
        for (int s = 0; s < loadedScenes.Length; s++)
        {
            loadedScenes[s] = SceneManager.GetSceneAt(s).name;
        }

        // load all scenes if it is not active
        for (int s = 0; s < this.m_InitialScenes.Length; s++)
        {
            if (!System.Array.Exists(loadedScenes, (scene) => scene == this.m_InitialScenes[s]))
            {
                SceneManager.LoadScene(this.m_InitialScenes[s], LoadSceneMode.Additive);
            }
        }
    }
}
