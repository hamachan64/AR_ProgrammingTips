using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ZuiZuiBox : MonoBehaviour
{
    [SerializeField] private VisualEffect _visualEffect;
#if UNITY_EDITOR
    // void Update()
    // {
    //     if (_visualEffect == null) return;

	// 	if (Input.GetKeyDown(KeyCode.Space))
	// 	{
    //         StartCoroutine("EffectChange", 0.5f);
    //     }
    //     if (Input.GetKeyDown(KeyCode.Return))
    //     {
    //         Debug.Log("Return");
    //         _visualEffect.SendEvent("Play");
    //     }
    //     if(Input.GetKeyDown(KeyCode.LeftShift))
    //     {
    //         StartCoroutine("ReturnEffect");
    //     }
    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         StartCoroutine("ReturnEffect");
    //     }
    // }
#endif
    
    public void Generate()
    {
        _visualEffect.SendEvent("Play");
    }
    
    public void SizeChange(float size)
    {
        StartCoroutine("EffectChange", size * 0.5f);
    }

    public void Turn(float value)
    {
        _visualEffect.SetFloat("InputImpactSpeed", value);
        StartCoroutine("ReturnEffect");
    }

    private IEnumerator EffectChange(float size)
	{
        _visualEffect.SetFloat("InputSize", size);
        _visualEffect.SetBool("ScaleEvent", true);
        yield return null;
        _visualEffect.SetBool("ScaleEvent", false);
    }

    private IEnumerator ReturnEffect()
    {
        _visualEffect.SetBool("UpdateFlag", true);
        yield return null;
        _visualEffect.SetBool("UpdateFlag", false);
    }

    public void Initialize()
    {
        _visualEffect.Stop();
        gameObject.SetActive(true);
    }

    public void Disable()
    {        
        _visualEffect.Stop();
        gameObject.SetActive(false);
    }
}

