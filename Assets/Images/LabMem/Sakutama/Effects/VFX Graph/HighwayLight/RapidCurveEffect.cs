using System.Collections;
using System.Collections.Generic;
using ARDrums.Effects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class RapidCurveEffect : MonoBehaviour
{
    private VisualEffect _vfx;

    // Start is called before the first frame update
    void Start()
    {
        _vfx = GetComponent<VisualEffect>();
    }
    
    public void Launch()
    {
        _vfx.SendEvent("Play");
    }
    
    public void Curve(float value = 1.0f)
    {
        _vfx.SetFloat("noise", value);
        // ゲーム時間からランダムな角度を生成
        float gameTimeMSec = Time.time * 100;
        float randX = Mathf.PerlinNoise(gameTimeMSec, 0);
        float randY = Mathf.PerlinNoise(0, gameTimeMSec);
        Vector2 randDir = new Vector2(randX * 2 - 1, (randY * 2 - 1) * 0.25f).normalized;

        _vfx.SetVector2("noiseDir", randDir);

        StartCoroutine(ResetNoiseNextFrame());
    }

    private IEnumerator ResetNoiseNextFrame()
    {
        yield return new WaitForEndOfFrame(); // 現在のフレームが終わるまで待つ
        yield return null; // 次のフレームの Update() まで待つ
        _vfx.SetFloat("noise", 0f);
    }

    public void Initialize()
    {
        _vfx = GetComponent<VisualEffect>();
    }

    public void Disable()
    {
        _vfx.Stop();
    }
}
