using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GateFinish : MonoBehaviour
{
    [Header("Win UI (assign in Inspector)")]
    public GameObject winPanel;
    public Button replayButton;
    public Button mainMenuButton;
    [Tooltip("Name of your Main Menu scene in Build Settings")]
    public string mainMenuSceneName = "MainMenu";

    private bool finished = false;

    void Awake()
    {
        if (winPanel) winPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            finished = true;

            // Stop timer / grade if present
            if (GameTimer.Instance != null)
                GameTimer.Instance.StopAndGrade();

            // Play WIN sfx if available
            if (SoundManager.Instance && SoundLibrary.Instance)
                SoundManager.Instance.PlaySoundEffect(
                    SoundLibrary.Instance.GetAudioClip(SoundLibrary.SoundEvents.WIN)
                );

            ShowWinPanel();
        }
    }

    private void ShowWinPanel()
    {
        // Freeze the game
        Time.timeScale = 0f;

        // Show panel and wire buttons
        if (winPanel) winPanel.SetActive(true);

        if (replayButton)
        {
            replayButton.onClick.RemoveAllListeners();
            replayButton.onClick.AddListener(ReplayLevel);
        }

        if (mainMenuButton)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        // Optional: focus the first button for keyboard/controller
        if (EventSystem.current && replayButton)
            EventSystem.current.SetSelectedGameObject(replayButton.gameObject);
    }

    private void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
            Debug.LogError("[GateFinish] Main Menu scene name not set.");
    }
}
