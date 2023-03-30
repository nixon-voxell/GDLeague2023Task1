using UnityEngine;
using System.Collections;

public class DestructableObstacle : MonoBehaviour
{
    [SerializeField] private float m_AnimDuration;
    [SerializeField] private AnimationCurve m_CreateAnimCurve;
    [SerializeField] private AnimationCurve m_DestroyAnimCurve;

    private bool destroyed = false;

    private void Start()
    {
        // Add this object to the LevelManager's DestructableObstacle array
        LevelManager levelManager = GameManager.Instance.LevelManager;
        if (levelManager != null)
        {
            levelManager.DestructableObstacles.Add(this);
        }

        this.destroyed = false;
    }

    public void DestroyObstacle()
    {
        if (destroyed == false)
        {
            this.StartCoroutine(this.DestroyAnimation());
            this.destroyed = true;
        }
    }

    public void CreateObstacle()
    {
        if (destroyed == true)
        {
            this.gameObject.SetActive(true);

            this.StartCoroutine(this.CreateAnimation());
            this.destroyed = false;
        }
    }

    private IEnumerator DestroyAnimation()
    {
        float startTime = Time.time;
        float timePassed = 0.0f;
        Transform trans = this.transform;

        while (timePassed < this.m_AnimDuration)
        {
            timePassed = Time.time - startTime;
            float scale = this.m_DestroyAnimCurve.Evaluate(timePassed / this.m_AnimDuration);
            trans.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForEndOfFrame();
        }

        this.gameObject.SetActive(false);
    }

    private IEnumerator CreateAnimation()
    {
        float startTime = Time.time;
        float timePassed = 0.0f;
        Transform trans = this.transform;

        while (timePassed < this.m_AnimDuration)
        {
            timePassed = Time.time - startTime;
            float scale = this.m_CreateAnimCurve.Evaluate(timePassed / this.m_AnimDuration);
            trans.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForEndOfFrame();
        }
    }
}
