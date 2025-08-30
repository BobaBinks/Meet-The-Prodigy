using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;

public class CutScene : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject background;
    [SerializeField] RawImage cutSceneScreen;

    public static Action OnCutSceneEnd;
    public static Action OnCutSceneStart;

    private void Start()
    {
        if (SoundManager.Instance)
        {
            SoundManager.Instance.StopMusic();
        }

        videoPlayer.Prepare();
    }

    private void Update()
    {
        if (!videoPlayer || videoPlayer.isPlaying)
            return;

        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
            OnCutSceneStart?.Invoke();
        }
    }

    private void OnEnable()
    {
        videoPlayer.loopPointReached += LoopPointReached;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= LoopPointReached;
    }

    private void LoopPointReached(VideoPlayer videoPlayer)
    {
        videoPlayer.Stop();

        if (background)
            background.SetActive(false);

        if (cutSceneScreen)
            cutSceneScreen.gameObject.SetActive(false);

        if (SoundManager.Instance)
            SoundManager.Instance.PlayMusic(SoundLibrary.Instance.GetAudioClip(SoundLibrary.Music.GAME));

        OnCutSceneEnd?.Invoke();
    }
}
