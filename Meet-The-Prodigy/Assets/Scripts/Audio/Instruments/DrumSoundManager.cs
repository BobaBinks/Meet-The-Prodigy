using UnityEngine;

public class DrumSoundManager : InstrumentSoundManager
{
    public override void PlaySound()
    {
        if (SoundManager.Instance && SoundLibrary.Instance)
        {
            SoundLibrary.Drum[] clips = (SoundLibrary.Drum[])System.Enum.GetValues(typeof(SoundLibrary.Drum));
            AudioClip audioClip = SoundLibrary.Instance.GetAudioClip((SoundLibrary.Drum)nextClipIndex);
            SoundManager.Instance.PlayInstrumentSoundEffect(audioClip);

            // update and wrap around index
            UpdateNextClipIndex(clips.Length);

            Debug.Log($"Clip {audioClip.name} played.");

            // reset index after delay
            ResetAfterDelay(delay);
        }
    }
}
