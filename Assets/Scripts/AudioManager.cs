using System.Collections;
using UnityEngine;

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
        audio.loop = true;
        audio.pitch = 1.0f;
        audio.Play();
    }

    public void PlayBgm(float audioStartTime)
    {
        audio.time = audioStartTime;
        audio.clip = bgm;
        audio.loop = true;
        audio.pitch = 1.0f;
        audio.Play();
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
