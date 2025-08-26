using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource instrumentSource;
    [SerializeField] AudioSource musicSource;

    [Header("Mixers")]
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioMixerGroup musicMixer;

    [Header("Pause Menu SnapShots")]
    [SerializeField] AudioMixerSnapshot audioMixerSnapshotUnpaused;
    [SerializeField] AudioMixerSnapshot audioMixerSnapshotPaused;

    bool isPause = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        if(musicSource)
            musicSource.volume = 0.5f;

        if (sfxSource)
            sfxSource.volume = 1f;
    }

    private void Start()
    {
        PlayMusic(SoundLibrary.Instance.GetAudioClip(SoundLibrary.Music.GAME),
            loop: true);
    }

    public float GetMusicVolume()
    {
        if (!musicSource)
            return 0f;

        return musicSource.volume;
    }

    public float GetSoundEffectsVolume()
    {
        if (!sfxSource)
            return 0f;

        return sfxSource.volume;
    }

    /// <summary>
    /// Callback function for Music volume change.
    /// </summary>
    /// <param name="vol">Music Volume value</param>
    public void OnMusicVolumeChange(float vol)
    {
        musicSource.volume = vol;
    }

    /// <summary>
    /// Callback function for SFX volume change
    /// </summary>
    /// <param name="vol">SFX Volume value</param>
    public void OnSoundEffectsVolumeChange(float vol)
    {
        sfxSource.volume = vol;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            OnPause(isPause);
            Debug.Log($"Paused: {isPause}");
        }
    }

    /// <summary>
    /// Call back function for pausing game.
    /// </summary>
    /// <param name="pause">Game is paused.</param>
    public void OnPause(bool pause)
    {
        if (pause)
            audioMixerSnapshotPaused.TransitionTo(0.1f);
        else
            audioMixerSnapshotUnpaused.TransitionTo(0.1f);
    }

    /// <summary>
    /// Play sound effect audio clip on sound effect audio source.
    /// </summary>
    /// <param name="sfx">Sound Effects clip</param>
    public void PlaySoundEffect(AudioClip sfx)
    {
        if (sfx == null)
            return;

        sfxSource.PlayOneShot(sfx);
    }

    public void PlayInstrumentSoundEffect(AudioClip instrumentSfx)
    {
        if (instrumentSfx == null)
            return;
        instrumentSource.clip = instrumentSfx;
        instrumentSource.Play();
    }

    /// <summary>
    /// Play sound effect audio clip on music audio source.
    /// </summary>
    /// <param name="clip">Music clip</param>
    /// <param name="loop">Music should loop</param>
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || musicSource == null)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
}
