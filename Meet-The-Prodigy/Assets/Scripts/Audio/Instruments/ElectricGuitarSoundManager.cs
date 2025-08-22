using System.Collections;
using UnityEngine;

public class ElectricGuitarSoundManager : InstrumentSoundManager
{
    public override void PlaySound()
    {
        if(SoundManager.Instance && SoundLibrary.Instance)
        {
            SoundLibrary.Electric_Guitar[] guitarClips = (SoundLibrary.Electric_Guitar[])System.Enum.GetValues(typeof(SoundLibrary.Electric_Guitar));
            AudioClip audioClip = SoundLibrary.Instance.GetAudioClip((SoundLibrary.Electric_Guitar)nextClipIndex);
            SoundManager.Instance.PlayInstrumentSoundEffect(audioClip);

            // update and wrap around index
            UpdateNextClipIndex(guitarClips.Length);

            Debug.Log($"Clip {audioClip.name} played.");

            // reset index after delay
            ResetAfterDelay(delay);
        }
    }
}
