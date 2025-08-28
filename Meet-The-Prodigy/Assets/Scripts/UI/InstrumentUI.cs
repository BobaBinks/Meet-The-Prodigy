using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Instrument { Drum = 1, Guitar = 2, Flute = 3 }

public class InstrumentUI : MonoBehaviour
{
    [Header("Icons (assign these three)")]
    public Image drumIcon;
    public Image guitarIcon;
    public Image fluteIcon;

    [Header("Icon Tint")]
    public Color iconActive = Color.white;
    public Color iconInactive = new Color(0.75f, 0.75f, 0.75f, 1f);

    [Header("Border (box outline)")]
    public Color borderActive = Color.black;
    public Color borderInactive = new Color(0f, 0f, 0f, 0.6f);
    public Vector2 borderThicknessActive = new Vector2(2f, -2f);
    public Vector2 borderThicknessInactive = new Vector2(1.5f, -1.5f);

    [Header("Key Badge")]
    public Color keyBadgeCol = new Color(1f, 1f, 1f, 0.65f);
    public Color keyActive = new Color(0f, 0f, 0f, 0.95f);
    public Color keyInactive = new Color(0f, 0f, 0f, 0.55f);

    [Header("Layout")]
    public Vector2 slotSize = new Vector2(120, 96);
    public Vector2 iconSize = new Vector2(78, 78);
    public Vector2 boxPadding = new Vector2(10, 10);
    public bool clipIconsToSlot = true;

    [Header("Switch pop")]
    [Range(1f, 1.5f)] public float popScale = 1.12f;
    [Range(0.05f, 0.25f)] public float popTime = 0.12f;

    Instrument current = Instrument.Drum;

    // runtime refs
    RectTransform drumSlot, guitarSlot, fluteSlot;
    Image drumBox, guitarBox, fluteBox;
    Outline drumOutline, guitarOutline, fluteOutline;
    TextMeshProUGUI drumKey, guitarKey, fluteKey;
    Image drumKeyBadge, guitarKeyBadge, fluteKeyBadge;

    static Sprite _onePixel;
    static Sprite OnePixelSprite
    {
        get
        {
            if (_onePixel == null)
            {
                var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                tex.SetPixel(0, 0, Color.white);
                tex.Apply();
                _onePixel = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            }
            return _onePixel;
        }
    }

    void Start()
    {
        drumSlot = EnsureSlot(drumIcon, "Slot_Drum", "1", out drumBox, out drumOutline, out drumKey, out drumKeyBadge);
        guitarSlot = EnsureSlot(guitarIcon, "Slot_Guitar", "2", out guitarBox, out guitarOutline, out guitarKey, out guitarKeyBadge);
        fluteSlot = EnsureSlot(fluteIcon, "Slot_Flute", "3", out fluteBox, out fluteOutline, out fluteKey, out fluteKeyBadge);

        ApplyVisuals(current, false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) SwitchTo(Instrument.Drum);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) SwitchTo(Instrument.Guitar);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) SwitchTo(Instrument.Flute);
    }

    public void SwitchTo(Instrument inst)
    {
        if (current == inst) return;
        current = inst;
        ApplyVisuals(current, true);
    }

    RectTransform EnsureSlot(Image icon, string slotName, string keyText,
                             out Image box, out Outline outline, out TextMeshProUGUI key, out Image keyBadge)
    {
        box = null; outline = null; key = null; keyBadge = null;
        if (icon == null) { Debug.LogWarning($"[InstrumentUI] Missing icon for {slotName}"); return null; }

        var parent = icon.rectTransform.parent as RectTransform;
        int oldIndex = icon.rectTransform.GetSiblingIndex();

        // Create / reuse slot
        RectTransform slotRT;
        if (icon.transform.parent != null && icon.transform.parent.name.StartsWith("Slot_"))
        {
            slotRT = icon.transform.parent as RectTransform;
        }
        else
        {
            var slotGO = new GameObject(slotName, typeof(RectTransform));
            slotRT = slotGO.GetComponent<RectTransform>();
            slotRT.SetParent(parent, false);
            slotRT.SetSiblingIndex(oldIndex);
            slotRT.anchorMin = slotRT.anchorMax = new Vector2(0.5f, 0.5f);
            slotRT.pivot = new Vector2(0.5f, 0.5f);
            slotRT.sizeDelta = slotSize;

            if (clipIconsToSlot) slotGO.AddComponent<RectMask2D>();

            icon.rectTransform.SetParent(slotRT, false);
        }

        // Box 
        var boxTF = slotRT.Find("Box");
        if (boxTF == null)
        {
            var go = new GameObject("Box", typeof(RectTransform), typeof(Image), typeof(Outline));
            var rt = go.GetComponent<RectTransform>();
            rt.SetParent(slotRT, false);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = boxPadding;
            rt.offsetMax = -boxPadding;

            var img = go.GetComponent<Image>();
            img.sprite = OnePixelSprite;
            img.type = Image.Type.Simple;
            img.color = new Color32(144, 150, 165, 255); 
            img.raycastTarget = false;
            rt.SetSiblingIndex(0); // behind icon

            var ol = go.GetComponent<Outline>();
            outline = ol;
            box = img;
        }
        else
        {
            box = boxTF.GetComponent<Image>();
            if (box == null) box = boxTF.gameObject.AddComponent<Image>();
            box.sprite = OnePixelSprite;
            box.type = Image.Type.Simple;
            box.color = new Color32(144, 150, 165, 255);
            var ol = boxTF.GetComponent<Outline>();
            if (ol == null) ol = boxTF.gameObject.AddComponent<Outline>();
            outline = ol;
        }

        // Icon styling
        icon.rectTransform.anchorMin = icon.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        icon.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        icon.rectTransform.sizeDelta = iconSize;
        icon.preserveAspect = true;
        icon.raycastTarget = false;
        icon.rectTransform.SetAsLastSibling();

        // Key badge + label (top-left)
        var badgeTF = slotRT.Find("KeyBadge");
        if (badgeTF == null)
        {
            var badgeGO = new GameObject("KeyBadge", typeof(RectTransform), typeof(Image));
            var brt = badgeGO.GetComponent<RectTransform>();
            brt.SetParent(slotRT, false);
            brt.anchorMin = brt.anchorMax = new Vector2(0f, 1f);
            brt.pivot = new Vector2(0f, 1f);
            brt.anchoredPosition = new Vector2(8f, -8f);
            brt.sizeDelta = new Vector2(24f, 22f);
            var bimg = badgeGO.GetComponent<Image>();
            bimg.sprite = OnePixelSprite;
            bimg.type = Image.Type.Simple;
            bimg.color = keyBadgeCol;
            keyBadge = bimg;

            var keyGO = new GameObject("Key", typeof(RectTransform));
            var rt = keyGO.GetComponent<RectTransform>();
            rt.SetParent(badgeGO.transform, false);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;

            var tmp = keyGO.AddComponent<TextMeshProUGUI>();
            tmp.text = keyText;
            tmp.fontSize = 18;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = keyInactive;
            tmp.textWrappingMode = TextWrappingModes.NoWrap;

            // outline for contrast
            tmp.fontMaterial.EnableKeyword("OUTLINE_ON");
            tmp.outlineColor = new Color(0, 0, 0, 0.55f);
            tmp.outlineWidth = 0.12f;

            key = tmp;
        }
        else
        {
            keyBadge = badgeTF.GetComponent<Image>();
            key = slotRT.Find("KeyBadge/Key")?.GetComponent<TextMeshProUGUI>();
        }

        return slotRT;
    }

    void ApplyVisuals(Instrument active, bool doPop)
    {
        SetSlot(active == Instrument.Drum, drumIcon, drumOutline, drumKey, drumKeyBadge, drumSlot, doPop);
        SetSlot(active == Instrument.Guitar, guitarIcon, guitarOutline, guitarKey, guitarKeyBadge, guitarSlot, doPop);
        SetSlot(active == Instrument.Flute, fluteIcon, fluteOutline, fluteKey, fluteKeyBadge, fluteSlot, doPop);
    }

    void SetSlot(bool isActive, Image icon, Outline outline, TextMeshProUGUI key, Image badge, RectTransform slot, bool doPop)
    {
        if (icon) icon.color = isActive ? iconActive : iconInactive;

        if (outline)
        {
            outline.effectColor = isActive ? borderActive : borderInactive;
            outline.effectDistance = isActive ? borderThicknessActive : borderThicknessInactive;
            outline.useGraphicAlpha = false;
        }

        if (key) key.color = isActive ? keyActive : keyInactive;
        if (badge) badge.color = new Color(keyBadgeCol.r, keyBadgeCol.g, keyBadgeCol.b, isActive ? keyBadgeCol.a : keyBadgeCol.a * 0.7f);

        if (isActive && slot != null && doPop)
            StartCoroutine(Pop(slot));
    }

    IEnumerator Pop(RectTransform rt)
    {
        float t = 0f;
        while (t < popTime) { t += Time.unscaledDeltaTime; rt.localScale = Vector3.Lerp(Vector3.one, Vector3.one * popScale, t / popTime); yield return null; }
        t = 0f;
        while (t < popTime) { t += Time.unscaledDeltaTime; rt.localScale = Vector3.Lerp(Vector3.one * popScale, Vector3.one, t / popTime); yield return null; }
        rt.localScale = Vector3.one;
    }
}
