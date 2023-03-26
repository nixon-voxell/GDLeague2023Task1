using UnityEngine;

[System.Serializable]
public class ObjectPool<T> where T : Object
{
    [SerializeField] private T Original;
    [SerializeField] private int m_Count;
    public T[] Pool;

    public int Count => this.m_Count;

    public void Initialize(Transform parent)
    {
#if UNITY_EDITOR
        Debug.Assert(this.Original != null, "Original object cannot be null.");
#endif
        this.Pool = new T[this.Count];
        for (int i = 0; i < this.Count; i++)
        {
            this.Pool[i] = Object.Instantiate(this.Original, Vector3.zero, Quaternion.identity, parent);
        }
    }

    public void Initialize()
    {
#if UNITY_EDITOR
        Debug.Assert(this.Original != null, "Original object cannot be null.");
#endif
        this.Pool = new T[this.Count];
        for (int i = 0; i < this.Count; i++)
        {
            this.Pool[i] = Object.Instantiate(this.Original, Vector3.zero, Quaternion.identity);
        }
    }
}
