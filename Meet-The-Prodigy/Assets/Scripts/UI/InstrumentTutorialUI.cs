using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstrumentTutorialUI : MonoBehaviour
{
    [Header("Panel & Controls")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Toggle dontShowAgainToggle;
    [SerializeField] private Button gotItButton;
    [SerializeField] private KeyCode reopenHotkey = KeyCode.I;
    [SerializeField] private bool pauseGameWhileOpen = true;

    [Header("Sprites")]
    [SerializeField] private Sprite drumSprite;
    [SerializeField] private Sprite guitarSprite;
    [SerializeField] private Sprite fluteSprite;

    [Header("Row UI (assign)")]
    [SerializeField] private Image drumImage;
    [SerializeField] private TMP_Text drumText;
    [SerializeField] private Image guitarImage;
    [SerializeField] private TMP_Text guitarText;
    [SerializeField] private Image fluteImage;
    [SerializeField] private TMP_Text fluteText;

    private const string PrefKey = "Tutorial_Level1_Shown";
    private bool isOpen;

    void Awake()
    {
        if (gotItButton) gotItButton.onClick.AddListener(Hide);
        if (tutorialPanel) tutorialPanel.SetActive(false);

        if (drumImage) drumImage.sprite = drumSprite;
        if (drumText) drumText.text = "Drum: Propels you backward from the cursor.";
        if (guitarImage) guitarImage.sprite = guitarSprite;
        if (guitarText) guitarText.text = "Guitar: Shatters cracked floors & orange walls.";
        if (fluteImage) fluteImage.sprite = fluteSprite;
        if (fluteText) fluteText.text = "Flute: Puts teachers & rats to sleep.";
    }

    void Start()
    {
        // Auto-show on Level 1 (no scene-name check needed if this script only exists in Level 1)
        bool already = PlayerPrefs.GetInt(PrefKey, 0) == 1;
        if (!already) Show();
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape)) Hide();
        if (!isOpen && Input.GetKeyDown(reopenHotkey)) Show(force: true);
    }

    public void Show(bool force = false)
    {
        if (!force && PlayerPrefs.GetInt(PrefKey, 0) == 1) return;
        if (tutorialPanel) tutorialPanel.SetActive(true);
        isOpen = true;
        if (pauseGameWhileOpen) Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (dontShowAgainToggle && dontShowAgainToggle.isOn)
        {
            PlayerPrefs.SetInt(PrefKey, 1);
            PlayerPrefs.Save();
        }
        if (tutorialPanel) tutorialPanel.SetActive(false);
        isOpen = false;
        if (pauseGameWhileOpen) Time.timeScale = 1f;
    }
}
