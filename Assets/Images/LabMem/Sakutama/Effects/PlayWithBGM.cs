using UnityEngine;
using UnityEngine.Playables;

public class PlayWithBGM : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private AudioSource audioSource;
    [Tooltip("If true, when the AudioSource starts playing the timeline will be synced and played.")]
    [SerializeField] private bool autoSyncOnAudioStart = true;
    [Tooltip("If true, timeline will be paused when audio stops playing.")]
    [SerializeField] private bool pauseWhenAudioStops = true;

    private bool _wasPlaying;

    void OnEnable()
    {
        if (director == null) director = GetComponent<PlayableDirector>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (director != null)
        {
            // reset timeline to start state and evaluate once to load references
            director.time = 0d;
            director.Evaluate();
        }

        _wasPlaying = audioSource != null && audioSource.isPlaying;
    }

    void OnDisable()
    {
        if (director != null && director.state == PlayState.Playing)
            director.Pause();
    }

    void Update()
    {
        if (!autoSyncOnAudioStart) return;
        if (audioSource == null || director == null) return;

        bool isPlaying = audioSource.isPlaying;
        // audio just started -> sync timeline to current audio time and play
        if (isPlaying && !_wasPlaying)
        {
            SyncTimelineToAudio();
        }

        // audio just stopped -> optionally pause timeline
        if (!isPlaying && _wasPlaying && pauseWhenAudioStops)
        {
            if (director.state == PlayState.Playing)
                director.Pause();
        }

        _wasPlaying = isPlaying;
    }

    // sync timeline time to the audio's current playback time and play
    public void SyncTimelineToAudio()
    {
        if (director == null || audioSource == null) return;
        director.time = audioSource.time;
        director.Play();
    }

    // convenience: start both from beginning
    public void PlayBothFromStart()
    {
        if (audioSource != null) audioSource.Play();
        if (director != null)
        {
            director.time = 0d;
            director.Play();
        }
    }

    // convenience: stop both
    public void StopBoth()
    {
        if (audioSource != null) audioSource.Stop();
        if (director != null) director.Stop();
    }
}
