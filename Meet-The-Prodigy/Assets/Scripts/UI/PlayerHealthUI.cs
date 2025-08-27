using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(HorizontalLayoutGroup))]
public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerHealth player;       // drag your Player here (faster). If empty, we’ll auto-find.

    [Header("Sprites")]
    public Sprite fullHeart;                    // red
    public Sprite emptyHeart;                   // grey

    [Header("Layout & Style")]
    public Vector2 heartSize = new Vector2(48, 48);
    public float spacing = 8f;
    public bool preserveAspect = true;
    public bool alsoTint = true;
    public Color fullTint = Color.white;
    public Color emptyTint = new Color(1f, 1f, 1f, 0.35f);

    readonly List<Image> hearts = new();
    HorizontalLayoutGroup layout;

    void Awake()
    {
        layout = GetComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.spacing = spacing;

        if (!player) player = ResolvePlayer();
        Rebuild(player ? player.maxHP : 3);
        Redraw();
    }

    void OnEnable()
    {
        if (!player) player = ResolvePlayer();
        if (player) player.OnHealthChanged += OnHealthChanged;
    }

    void OnDisable()
    {
        if (player) player.OnHealthChanged -= OnHealthChanged;
    }

    void Update()
    {
        // Cheap guard in case maxHP changes at runtime or player is assigned late.
        if (!player) { player = ResolvePlayer(); if (player) HookAndSync(); return; }
        if (hearts.Count != player.maxHP) Rebuild(player.maxHP);
    }

    // --- callbacks / draw ---

    void OnHealthChanged(int current, int max)
    {
        if (hearts.Count != max) Rebuild(max);
        Draw(current, max);
    }

    void Redraw()
    {
        if (!player) return;
        Draw(player.currentHP, player.maxHP);
    }

    void Draw(int current, int max)
    {
        int clamped = Mathf.Clamp(current, 0, max);
        for (int i = 0; i < hearts.Count; i++)
        {
            bool filled = i < clamped;
            var img = hearts[i];

            if (fullHeart && emptyHeart)
                img.sprite = filled ? fullHeart : emptyHeart;

            img.color = alsoTint ? (filled ? fullTint : emptyTint) : Color.white;
        }
    }

    void Rebuild(int count)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
        hearts.Clear();

        for (int i = 0; i < Mathf.Max(1, count); i++)
        {
            var go = new GameObject($"Heart{i + 1}", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(transform, false);

            var img = go.GetComponent<Image>();
            img.preserveAspect = preserveAspect;
            img.sprite = fullHeart ? fullHeart : emptyHeart;
            ((RectTransform)go.transform).sizeDelta = heartSize;

            hearts.Add(img);
        }
        layout.spacing = spacing;
    }

    void HookAndSync()
    {
        player.OnHealthChanged += OnHealthChanged;
        Rebuild(player.maxHP);
        Redraw();
    }

    PlayerHealth ResolvePlayer()
    {
        if (PlayerHealth.Instance) return PlayerHealth.Instance;
#if UNITY_2022_2_OR_NEWER
        return Object.FindFirstObjectByType<PlayerHealth>(FindObjectsInactive.Exclude);
#else
        return Object.FindObjectOfType<PlayerHealth>();
#endif
    }
}
