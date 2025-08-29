using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const string KEY_UNLOCKED_COUNT = "UnlockedLevel";
    private const string KEY_REACHED_INDEX  = "ReachedIndex";

    // Guards the reset so it happens only once per app run
    private static bool s_resetDoneThisSession = false;

    public void PlayGame()
    {
        // load first playable level (build index 1)
        SceneManager.LoadSceneAsync(1);
    }

    void Awake()
    {
        if (!s_resetDoneThisSession)
        {
            // Reset progress ONLY once per application run
            PlayerPrefs.SetInt(KEY_UNLOCKED_COUNT, 1);
            PlayerPrefs.SetInt(KEY_REACHED_INDEX, 1);
            PlayerPrefs.Save();

            s_resetDoneThisSession = true;
            Debug.Log("Application launch -> Progress reset to Level 1 (once).");
        }
        else
        {
            Debug.Log("MainMenu loaded again; progress preserved.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
