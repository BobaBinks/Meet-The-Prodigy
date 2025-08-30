using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons; 
    private const string KEY_UNLOCKED_COUNT = "UnlockedLevel";

    private void Awake()
    {
        int unlockedCount = PlayerPrefs.GetInt(KEY_UNLOCKED_COUNT, 1);
        int maxToEnable = Mathf.Clamp(unlockedCount, 0, buttons.Length);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                buttons[i].interactable = false;
                buttons[i].onClick.RemoveAllListeners();
            }
        }

        for (int i = 0; i < maxToEnable; i++)
        {
            if (buttons[i] == null) continue;

            int levelNumber = i + 1;
            buttons[i].interactable = true;
            buttons[i].onClick.AddListener(() => OpenLevel(levelNumber));
        }

        Debug.Log("[LevelMenu] UnlockedLevel=" + unlockedCount + " (enabled " + maxToEnable + " buttons)");
    }

    public void OpenLevel(int levelNumber)
    {
        if (SoundManager.Instance && SoundLibrary.Instance)
            SoundManager.Instance.PlayMusic(SoundLibrary.Instance.GetAudioClip(SoundLibrary.Music.GAME));
        string levelName = $"Level {levelNumber}";
        SceneManager.LoadScene(levelName);
    }
}
