using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject winPanel;
    public Button replayButton;
    public Button mainMenuButton;
    public Button nextLevelButton;

    [Header("Scenes")]
    [Tooltip("Main Menu scene name (must be in Build Settings).")]
    public string mainMenuSceneName = "MainMenu";

    [Tooltip("RECOMMENDED: Ordered list of level scene names (e.g., Level 1, Level 2, Level 3). If empty, we fall back to Build Index order.")]
    public string[] levelSceneNames;

    [Tooltip("If true and levelSceneNames is empty, use Build Settings order to find the next level.")]
    public bool useBuildIndexOrderFallback = true;

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

        if (nextLevelButton)
        {
            nextLevelButton.onClick.RemoveAllListeners();
            // Enable/disable based on whether a next level exists
            bool hasNext = TryGetNextLevelName(out string _);
            nextLevelButton.gameObject.SetActive(hasNext);
            if (hasNext)
                nextLevelButton.onClick.AddListener(GoToNextLevel);
        }

        if (EventSystem.current && (replayButton || nextLevelButton))
        {
            // Prefer Next Level if available; else Replay
            GameObject first =
                (nextLevelButton && nextLevelButton.gameObject.activeSelf) ? nextLevelButton.gameObject :
                (replayButton ? replayButton.gameObject : null);
            if (first) EventSystem.current.SetSelectedGameObject(first);
        }
    }

    private void ReplayLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
            Debug.LogError("[WinUI] Main menu scene name not set!");
    }

    private void GoToNextLevel()
    {
        if (!TryGetNextLevelName(out string next))
        {
            Debug.LogWarning("[WinUI] No next level found.");
            return;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(next);
    }


    private bool TryGetNextLevelName(out string nextLevelName)
    {
        string current = SceneManager.GetActiveScene().name;

        // --- Mode A: explicit ordered list
        if (levelSceneNames != null && levelSceneNames.Length > 0)
        {
            int idx = System.Array.IndexOf(levelSceneNames, current);
            if (idx >= 0 && idx + 1 < levelSceneNames.Length)
            {
                nextLevelName = levelSceneNames[idx + 1];
                return true;
            }
        }

        // --- Mode B: build index fallback
        if (useBuildIndexOrderFallback)
        {
            int currentIdx = SceneManager.GetActiveScene().buildIndex;
            int total = SceneManager.sceneCountInBuildSettings;
            if (currentIdx + 1 < total)
            {
                // Get scene path from build and then extract the scene name
                string path = SceneUtility.GetScenePathByBuildIndex(currentIdx + 1);
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                nextLevelName = name;
                return true;
            }
        }

        nextLevelName = null;
        return false;
    }
}
