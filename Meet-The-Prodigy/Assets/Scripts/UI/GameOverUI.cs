using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

[DisallowMultipleComponent]
public class GameOverUI : MonoBehaviour
{
    [Header("Assign from Canvas (or leave blank to auto-find)")]
    [SerializeField] private CanvasGroup panel;      // GameOverPanel (CanvasGroup)
    [SerializeField] private TMP_Text titleText;     // Title (TMP), optional
    [SerializeField] private Button againButton;     // "Again"
    [SerializeField] private Button menuButton;      // "Menu"

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Pop Animation")]
    public bool popOnShow = true;
    [Range(1f, 1.3f)] public float popScale = 1.05f;
    [Range(0.05f, 0.5f)] public float popTime = 0.18f; // unscaled time

    bool isOpen;
    Transform panelTransform;

    void Awake()
    {
        // --- Auto-find common children ---
        if (!panel)
            panel = transform.Find("GameOverPanel")?.GetComponent<CanvasGroup>();

        if (panel)
        {
            panelTransform = panel.transform;

            // Title: prefer an object named "Title" or "GameOverTitle", else first TMP under panel
            if (!titleText)
            {
                var t = panel.transform.Find("Title") ?? panel.transform.Find("GameOverTitle");
                titleText = t ? t.GetComponent<TMP_Text>() : panel.GetComponentInChildren<TMP_Text>(true);
            }

            // Buttons: search anywhere under panel so it works whether you use ButtonsRow or not
            var buttons = panel.GetComponentsInChildren<Button>(true);
            if (!againButton)
                againButton = buttons.FirstOrDefault(b => b.name.ToLower().Contains("again") ||
                                                          b.name.ToLower().Contains("retry") ||
                                                          b.name.ToLower().Contains("restart"));
            if (!menuButton)
                menuButton = buttons.FirstOrDefault(b => b.name.ToLower().Contains("menu") ||
                                                          b.name.ToLower().Contains("main"));
        }

        // Wire button handlers if found
        if (againButton)
        {
            againButton.onClick.RemoveAllListeners();
            againButton.onClick.AddListener(RestartLevel);
        }
        if (menuButton)
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(GoToMenu);
        }

        HidePanelInstant();
    }

    void OnEnable()
    {
        var ph = PlayerHealth.Instance ?? FindPlayer();
        if (ph != null) ph.onDeath.AddListener(Show);
    }

    void OnDisable()
    {
        var ph = PlayerHealth.Instance;
        if (ph != null) ph.onDeath.RemoveListener(Show);
    }

    void Update()
    {
        // Fallback: if we missed event due to order, show when player is dead
        if (!isOpen)
        {
            var ph = PlayerHealth.Instance ?? FindPlayer();
            if (ph != null && ph.IsDead) Show();
        }
    }

    // ---------- UI logic ----------

    public void Show()
    {
        if (isOpen || panel == null) return;
        isOpen = true;

        Time.timeScale = 0f;

        panel.gameObject.SetActive(true);
        panel.alpha = 1f;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        if (titleText) titleText.text = "Game Over";

        if (popOnShow && panelTransform != null)
        {
            panelTransform.localScale = Vector3.one;
            StartCoroutine(Pop(panelTransform, popScale, popTime));
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogWarning("[GameOverUI] Set Main Menu Scene Name in the inspector.");
            return;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // ---------- helpers ----------

    void HidePanelInstant()
    {
        if (!panel) return;
        panel.gameObject.SetActive(true); // keep active so layout is ready
        panel.alpha = 0f;
        panel.interactable = false;
        panel.blocksRaycasts = false;
        if (panelTransform) panelTransform.localScale = Vector3.one;
        isOpen = false;
    }

    System.Collections.IEnumerator Pop(Transform target, float scale, float totalDuration)
    {
        float half = Mathf.Max(0.0001f, totalDuration * 0.5f);

        float t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, t / half);
            yield return null;
        }

        t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(Vector3.one * scale, Vector3.one, t / half);
            yield return null;
        }

        target.localScale = Vector3.one;
    }

    PlayerHealth FindPlayer()
    {
#if UNITY_2022_2_OR_NEWER
        return Object.FindFirstObjectByType<PlayerHealth>(FindObjectsInactive.Exclude);
#else
        return Object.FindObjectOfType<PlayerHealth>();
#endif
    }
}
