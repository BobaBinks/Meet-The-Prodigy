using UnityEngine;
using UnityEngine.SceneManagement;

public class GateFinish : MonoBehaviour
{
    private const string KEY_UNLOCKED_COUNT = "UnlockedLevel"; 
    private const string KEY_REACHED_INDEX  = "ReachedIndex";  
    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;
        if (!other.CompareTag("Player")) return;

        finished = true;

        if (GameTimer.Instance != null)
            GameTimer.Instance.StopAndGrade();

        UnlockNextLevel();

        if (SoundManager.Instance && SoundLibrary.Instance)
        {
            SoundManager.Instance.PlaySoundEffect(
                SoundLibrary.Instance.GetAudioClip(SoundLibrary.SoundEvents.WIN)
            );
            SoundManager.Instance.StopMusic();
        }

        if (WinUI.Instance != null)
            WinUI.Instance.Show();
    }

    private void UnlockNextLevel()
    {

        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextBuildIndex    = currentBuildIndex + 1;

        if (nextBuildIndex >= SceneManager.sceneCountInBuildSettings)
            return;

        int highestReached = PlayerPrefs.GetInt(KEY_REACHED_INDEX, 1);
        if (currentBuildIndex >= highestReached)
        {
            PlayerPrefs.SetInt(KEY_REACHED_INDEX, currentBuildIndex + 1);
            int unlockedCount = PlayerPrefs.GetInt(KEY_UNLOCKED_COUNT, 1);
            PlayerPrefs.SetInt(KEY_UNLOCKED_COUNT, unlockedCount + 1);
            PlayerPrefs.Save();
            Debug.Log("[Unlock] ReachedIndex -> " + (currentBuildIndex + 1) + ", UnlockedLevel -> " + (unlockedCount + 1));
        }
        else
        {
            Debug.Log("Level was already counted).");
        }
    }
}
