using UnityEngine;
using TMPro;
using UnityEngine.Events;

[System.Serializable] public class StringEvent : UnityEvent<string> { }

public class GameTimer : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text gradeText;
    public TMP_Text bestText;

    [Header("Behaviour")]
    public bool autoStart = true;
    public string levelId = "Level1";   // PlayerPrefs key suffix

    [Header("Grade thresholds (seconds)")]
    public float aMax = 60f;            // A if t < 60
    public float bMax = 120f;           // B if t < 120
    public float cMax = 180f;           // C if t < 180
    public float dMax = 240f;           // D if t < 240
                                        // F if t >= 240

    [Header("Grade Visuals")]
    public Color aColor = new Color(0.20f, 0.85f, 0.30f); // green
    public Color bColor = new Color(0.25f, 0.60f, 1.00f); // blue
    public Color cColor = new Color(1.00f, 0.85f, 0.25f); // yellow
    public Color dColor = new Color(1.00f, 0.55f, 0.20f); // orange
    public Color fColor = new Color(1.00f, 0.25f, 0.25f); // red
    [Range(1f, 1.5f)] public float gradePopScale = 1.12f;
    [Range(0.05f, 0.25f)] public float gradePopTime = 0.12f;

    [Header("Events")]
    public StringEvent OnGraded;        // fires with "A/B/C/D/F" when finished

    // Runtime
    public static GameTimer Instance { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsFinished { get; private set; }
    public float ElapsedSeconds { get; private set; }

    float _best = -1f; // lower is better

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (PlayerPrefs.HasKey(BestKey())) _best = PlayerPrefs.GetFloat(BestKey(), -1f);

        ElapsedSeconds = 0f;
        if (gradeText) gradeText.text = "";
        UpdateTimerLabel();
        UpdateBestLabel();
    }

    void Start()
    {
        if (autoStart) StartTimer();
    }

    void Update()
    {
        if (!IsRunning || IsFinished) return;

        ElapsedSeconds += Time.deltaTime;
        UpdateTimerLabel();
    }


    public void StartTimer() { IsRunning = true; IsFinished = false; }

    public void PauseTimer() { IsRunning = false; }

    public void ResumeTimer() { if (!IsFinished) IsRunning = true; }

    public void ResetTimer()
    {
        IsRunning = false;
        IsFinished = false;
        ElapsedSeconds = 0f;
        if (gradeText) { gradeText.text = ""; gradeText.rectTransform.localScale = Vector3.one; }
        UpdateTimerLabel();
    }

    public void StopAndGrade()
    {
        if (IsFinished) return;

        IsRunning = false;
        IsFinished = true;

        string grade = ComputeGrade(ElapsedSeconds);
        if (gradeText)
        {
            gradeText.text = grade;
            gradeText.color = ColorForGrade(grade);
            StartCoroutine(Pop(gradeText.rectTransform, gradePopScale, gradePopTime));
        }

        // Save best
        if (_best < 0f || ElapsedSeconds < _best)
        {
            _best = ElapsedSeconds;
            PlayerPrefs.SetFloat(BestKey(), _best);
            PlayerPrefs.Save();
        }
        UpdateBestLabel();

        OnGraded?.Invoke(grade);
    }


    string ComputeGrade(float t)
    {
        if (t < aMax) return "A";
        if (t < bMax) return "B";
        if (t < cMax) return "C";
        if (t < dMax) return "D";
        return "F";
    }

    Color ColorForGrade(string g)
    {
        switch (g)
        {
            case "A": return aColor;
            case "B": return bColor;
            case "C": return cColor;
            case "D": return dColor;
            default: return fColor;
        }
    }

    System.Collections.IEnumerator Pop(RectTransform rt, float scale, float t)
    {
        float a = 0f;
        while (a < t) { a += Time.unscaledDeltaTime; rt.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, a / t); yield return null; }
        a = 0f;
        while (a < t) { a += Time.unscaledDeltaTime; rt.localScale = Vector3.Lerp(Vector3.one * scale, Vector3.one, a / t); yield return null; }
        rt.localScale = Vector3.one;
    }

    void UpdateTimerLabel()
    {
        if (!timerText) return;
        timerText.text = FormatTime(ElapsedSeconds);
    }

    void UpdateBestLabel()
    {
        if (!bestText) return;
        if (_best < 0f) { bestText.text = "Best: --:--"; return; }
        bestText.text = "Best: " + FormatTime(_best);
    }

    string BestKey() => $"BEST_TIME_{levelId}_STOPWATCH";

    string FormatTime(float seconds)
    {
        if (seconds < 0f) seconds = 0f;
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        return $"{m:00}:{s:00}";
    }
}
