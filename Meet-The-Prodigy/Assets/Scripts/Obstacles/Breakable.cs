using UnityEngine;

public class Breakable : MonoBehaviour, IGuitarAffectable
{
    public void ApplyGuitarEffect(float force, Vector2 direction)
    {
        if(SoundManager.Instance && SoundLibrary.Instance)
            SoundManager.Instance.PlaySoundEffect(SoundLibrary.Instance.GetAudioClip(SoundLibrary.Obstacle.WALL_CRUMBLE));

        Destroy(this.gameObject);
    }
}
