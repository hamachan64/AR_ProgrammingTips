using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    new AudioSource audio;
    [SerializeField] AudioClip opening;
    [SerializeField] AudioClip bgm;
    [SerializeField] AudioClip findCardBgm;
    [SerializeField] AudioClip startPerformanceBgm;
    [SerializeField] AudioClip immersiveParticleBgm;

    private bool findCardFlag = false;

    void Start()
    {
        audio = this.GetComponent<AudioSource>();
        audio.clip = opening;
        audio.volume = 0f;
        audio.Play();
        audio.DOFade(1f, 1.0f);
    }

    public void PlayBgm(float audioStartTime)
    {
        RunSequenceAsyncBgm(audioStartTime).Forget();
    }

    private async UniTask RunSequenceAsyncBgm(float audioStartTime = 0f)
    {
        // すでにフェードアウト中のTweenがあれば止める
        audio.DOKill();

        // --- opening → フェードアウト ---
        await audio.DOFade(0f, 1.0f)
            .SetEase(Ease.OutSine)
            .AsyncWaitForCompletion();

        audio.Stop();

        // --- 新しい bgm をセット ---
        audio.clip = bgm;
        audio.time = audioStartTime;
        audio.volume = 0f;
        audio.loop = true;
        audio.Play();

        // --- フェードイン ---
        await audio.DOFade(1f, 1.0f)
            .SetEase(Ease.InSine)
            .AsyncWaitForCompletion();
    }

    public void PlayFindCardBgm()
    {
        // if (findCardFlag)
        // {
        //     return;
        // }
        // findCardFlag = true;
        audio.Stop();
        audio.loop = false;
        audio.pitch = 1.0f;
        audio.PlayOneShot(findCardBgm);
        // StartCoroutine(RePlayBgm());
    }

    IEnumerator RePlayBgm()
    {
        while (audio.isPlaying)
        {
            yield return null;
        }
        PlayBgm(5.0f);
        findCardFlag = false;
    }

    public void PlayPerformanceBgm()
    {
        audio.Pause();
        audio.loop = false;
        audio.pitch = 0.4f;
        audio.clip = startPerformanceBgm;
        audio.Play();
    }

    public void ParticleBgm()
    {
        audio.Pause();
        audio.loop = false;
        audio.pitch = 1.0f;
        audio.clip = immersiveParticleBgm;
        audio.Play();
    }

}
