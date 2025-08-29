using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class SettingsManager : MonoBehaviour
{
    [Header("UI Roots")]
    [SerializeField] private GameObject pauseHub;      // 3 buttons only (Settings / Controls / Menu)
    [SerializeField] private GameObject settingsPage;  // volume sliders + Back
    [SerializeField] private GameObject controlsPage;  // movement UI + Back

    [Header("Sliders (0..1)")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Main Menu")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool paused;
    public static Action<bool> OnPause;

    void Awake()
    {
        // Start clean
        if (pauseHub) pauseHub.SetActive(false);
        if (settingsPage) settingsPage.SetActive(false);
        if (controlsPage) controlsPage.SetActive(false);
        Time.timeScale = 1f;
    }

    void Start()
    {
        // Load & apply saved volumes (uses SoundManager for Music/SFX; Master via AudioListener)
        float master = PlayerPrefs.GetFloat("pref_Master", 0.8f);
        float music = PlayerPrefs.GetFloat("pref_Music", 0.8f);
        float sfx = PlayerPrefs.GetFloat("pref_SFX", 0.8f);

        if (masterSlider) { masterSlider.SetValueWithoutNotify(master); masterSlider.onValueChanged.AddListener(OnMasterChanged); }
        if (musicSlider) { musicSlider.SetValueWithoutNotify(music); musicSlider.onValueChanged.AddListener(OnMusicChanged); }
        if (sfxSlider) { sfxSlider.SetValueWithoutNotify(sfx); sfxSlider.onValueChanged.AddListener(OnSfxChanged); }

        OnMasterChanged(master);
        OnMusicChanged(music);
        OnSfxChanged(sfx);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                OpenHub();
            }
            else
            {
                // If on a subpage, go back to hub; otherwise close pause
                if ((settingsPage && settingsPage.activeSelf) || (controlsPage && controlsPage.activeSelf))
                    ShowHub();
                else
                    CloseAll();
            }
        }
    }

    // ---------- State control ----------
    private void OpenHub()
    {
        paused = true;
        OnPause?.Invoke(paused);
        if (SoundManager.Instance) SoundManager.Instance.OnPause(true);
        Time.timeScale = 0f;
        ShowHub();
    }

    private void ShowHub()
    {
        if (pauseHub) pauseHub.SetActive(true);
        if (settingsPage) settingsPage.SetActive(false);
        if (controlsPage) controlsPage.SetActive(false);
    }

    public void CloseAll()
    {
        paused = false;
        OnPause?.Invoke(paused);
        Time.timeScale = 1f;
        if (pauseHub) pauseHub.SetActive(false);
        if (settingsPage) settingsPage.SetActive(false);
        if (controlsPage) controlsPage.SetActive(false);
        if (SoundManager.Instance) SoundManager.Instance.OnPause(false);
    }

    // ---------- Hub buttons ----------
    public void OnClick_Settings()
    {
        if (!paused) OpenHub();
        if (pauseHub) pauseHub.SetActive(false);
        if (settingsPage) settingsPage.SetActive(true);
        if (controlsPage) controlsPage.SetActive(false);
    }

    public void OnClick_Controls()
    {
        if (!paused) OpenHub();
        if (pauseHub) pauseHub.SetActive(false);
        if (settingsPage) settingsPage.SetActive(false);
        if (controlsPage) controlsPage.SetActive(true);
    }

    public void OnClick_Menu()
    {
        Time.timeScale = 1f;
        if (SoundManager.Instance) SoundManager.Instance.OnPause(false);
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // ---------- Page "Back" buttons ----------
    public void OnClick_Back()
    {
        // back to hub, stay paused
        ShowHub();
    }

    // ---------- Volume handlers ----------
    private void OnMasterChanged(float v)
    {
        AudioListener.volume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat("pref_Master", v);
    }

    private void OnMusicChanged(float v)
    {
        if (SoundManager.Instance)
            SoundManager.Instance.OnMusicVolumeChange(Mathf.Clamp01(v));
        PlayerPrefs.SetFloat("pref_Music", v);
    }

    private void OnSfxChanged(float v)
    {
        if (SoundManager.Instance)
            SoundManager.Instance.OnSoundEffectsVolumeChange(Mathf.Clamp01(v));
        PlayerPrefs.SetFloat("pref_SFX", v);
    }

    void OnDisable()
    {
        if (SoundManager.Instance) SoundManager.Instance.OnPause(false);
        if (Time.timeScale == 0f) Time.timeScale = 1f;
    }
}
