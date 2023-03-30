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
            levelManager.DestructableObstacles.Add(this);
        }
    }

    public void DestroyObstacle()
    {
        if (m_Animator != null)
            m_Animator.SetTrigger(m_DestroyTrigger);

        StartCoroutine(DisableGameObjectAfterDelay(m_Animator.GetCurrentAnimatorStateInfo(0).length));
    }

    public void CreateObstacle()
    {
        gameObject.SetActive(true);

        // Temporarily check null to ensure error doesn't pop up
        if (m_Animator != null)
            m_Animator.SetTrigger(m_CreateTrigger);
    }

    private IEnumerator DisableGameObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}