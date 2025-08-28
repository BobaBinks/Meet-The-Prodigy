using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject winPanel;
    public Button replayButton;
    public Button mainMenuButton;
    public string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (winPanel) winPanel.SetActive(false);
    }

    public void Show()
    {
        // Freeze gameplay
        Time.timeScale = 0f;

        if (winPanel) winPanel.SetActive(true);

        // Wire buttons
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

        // Auto-select replay button for keyboard/controller
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
            Debug.LogError("[WinUI] Main menu scene name not set!");
    }
}
