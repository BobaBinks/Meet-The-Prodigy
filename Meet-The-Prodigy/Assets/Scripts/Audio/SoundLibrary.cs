using UnityEngine;
using System.Collections.Generic;

public class SoundLibrary: MonoBehaviour
{
    public static SoundLibrary Instance { get; private set; }

    public enum Player
    {
        PLAYER_HIT,
        FOOTSTEP_1,
        FOOTSTEP_2,
        FOOTSTEP_3,
        FOOTSTEP_4,
        FOOTSTEP_5
    }

    public enum Enemy
    {
        FOOTSTEP_1,
    }

    public enum Electric_Guitar 
    {
        BEAT_1,
        BEAT_2,
        BEAT_3,
        BEAT_4,
        BEAT_5,
        BEAT_6,
    }

    public enum Flute
    {
        BEAT_1,
        BEAT_2,
        BEAT_3,
        BEAT_4,
        BEAT_5,
    }

    public enum Drum
    {
        BEAT_1,
        BEAT_2,
        BEAT_3,
        BEAT_4,
        BEAT_5,
        BEAT_6,
        BEAT_7,
    }

    public enum Music
    {
        MENU,
        GAME,
    }

    #region Clips
    [Header("Player Clips")]
    [SerializeField] List<AudioClip> playerClips;

    [Header("Enemy Clips")]
    [SerializeField] List<AudioClip> enemyClips;

    [Header("Music")]
    [SerializeField] List<AudioClip> musicClips;

    [Header("Electric Guitar")]
    [SerializeField] List<AudioClip> electricGuitarClips;

    [Header("Flute")]
    [SerializeField] List<AudioClip> fluteClips;

    [Header("Drum")]
    [SerializeField] List<AudioClip> drumClips;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(this);
    }

    #region GetAudioClip functions

    public AudioClip GetAudioClip(Player sound)
    {
        int index = (int)sound;
        if (index >= 0 && index < playerClips.Count)
            return playerClips[index];

        Debug.LogWarning($"Player sound '{sound}' not assigned.");
        return null;
    }

    public AudioClip GetAudioClip(Enemy sound)
    {
        int index = (int)sound;
        if (index >= 0 && index < enemyClips.Count)
            return enemyClips[index];

        Debug.LogWarning($"Enemy sound '{sound}' not assigned.");
        return null;
    }

    public AudioClip GetAudioClip(Music sound)
    {
        int index = (int)sound;
        if (index >= 0 && index < musicClips.Count)
            return musicClips[index];

        Debug.LogWarning($"Music sound '{sound}' not assigned.");
        return null;
    }

    public AudioClip GetAudioClip(Electric_Guitar sound)
    {
        int index = (int)sound;
        if (index >= 0 && index < electricGuitarClips.Count)
            return electricGuitarClips[index];

        Debug.LogWarning($"Electric Guitar sound '{sound}' not assigned.");
        return null;
    }

    public AudioClip GetAudioClip(Flute sound)
    {
        int index = (int)sound;
        if (index >= 0 && index < fluteClips.Count)
            return fluteClips[index];

        Debug.LogWarning($"Flute sound '{sound}' not assigned.");
        return null;
    }

    public AudioClip GetAudioClip(Drum sound)
    {
        int index = (int)sound;
        if (index >= 0 && index < drumClips.Count)
            return drumClips[index];

        Debug.LogWarning($"Drum sound '{sound}' not assigned.");
        return null;
    }

    #endregion
}