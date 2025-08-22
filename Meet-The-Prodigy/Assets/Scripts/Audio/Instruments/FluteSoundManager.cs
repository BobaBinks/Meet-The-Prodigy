using UnityEngine;

public class FluteSoundManager : InstrumentSoundManager
{
    public override void PlaySound()
    {
        if (SoundManager.Instance && SoundLibrary.Instance)
        {
            SoundLibrary.Flute[] clips = (SoundLibrary.Flute[])System.Enum.GetValues(typeof(SoundLibrary.Flute));
            AudioClip audioClip = SoundLibrary.Instance.GetAudioClip((SoundLibrary.Flute)nextClipIndex);
            SoundManager.Instance.PlayInstrumentSoundEffect(audioClip);

            // update and wrap around index
            UpdateNextClipIndex(clips.Length);

            Debug.Log($"Clip {audioClip.name} played.");

            // reset index after delay
            ResetAfterDelay(delay);
        }
    }
}
