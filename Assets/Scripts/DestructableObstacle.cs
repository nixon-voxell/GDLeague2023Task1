using UnityEngine;
using System.Collections;

public class DestructableObstacle : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    private static readonly int m_DestroyTrigger = Animator.StringToHash("Destroy");
    private static readonly int m_CreateTrigger = Animator.StringToHash("Create");

    private void Start()
    {
        // Add this object to the LevelManager's DestructableObstacle array
        LevelManager levelManager = GameManager.Instance.LevelManager;
        if (levelManager != null)
        {
            levelManager.DestructableObstacle.Add(this);
        }
    }

    public void DestroyObstacle()
    {
        m_Animator.SetTrigger(m_DestroyTrigger);

        StartCoroutine(DisableGameObjectAfterDelay(m_Animator.GetCurrentAnimatorStateInfo(0).length));
    }

    public void CreateObstacle()
    {
        gameObject.SetActive(true);

        m_Animator.SetTrigger(m_CreateTrigger);
    }

    private IEnumerator DisableGameObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}