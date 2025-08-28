using UnityEngine;

public class GateFinish : MonoBehaviour
{
    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;
        if (!other.CompareTag("Player")) return;

        finished = true;

        // Stop timer / grading if present
        if (GameTimer.Instance != null)
            GameTimer.Instance.StopAndGrade();

        // Play WIN sfx if available
        if (SoundManager.Instance && SoundLibrary.Instance)
            SoundManager.Instance.PlaySoundEffect(
                SoundLibrary.Instance.GetAudioClip(SoundLibrary.SoundEvents.WIN)
            );

        // Notify WinUI
        WinUI.Instance.Show();
    }
}
