using UnityEngine;
using UnityEngine.VFX;

public class SkillTest : MonoBehaviour
{
    public VisualEffect VisualEffect;
    public VisualEffectAsset VisualEffectAsset;

    // Start is called before the first frame update
    void Start()
    {
        this.VisualEffect.visualEffectAsset = this.VisualEffectAsset;
        this.VisualEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
