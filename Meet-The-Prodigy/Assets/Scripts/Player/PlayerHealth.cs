using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [Header("Health")]
    [Min(1)] public int maxHP = 3;
    public int currentHP;

    [Header("Damage / I-frames")]
    [Min(0f)] public float invincibleDuration = 1f;
    float invincibleUntil = -999f;

    [Header("Optional")]
    public MonoBehaviour[] disableOnDeath;
    public UnityEvent onDamaged;
    public UnityEvent onDeath;

    // (current, max)
    public Action<int, int> OnHealthChanged;

    public bool IsDead { get; private set; }

    // --- lifecycle ---

    void Reset() { SnapToValidValues(editMode: true); }
    void OnValidate() { SnapToValidValues(editMode: true); }

    void Awake()
    {
        Instance = this;
        SnapToValidValues(editMode: false);
    }

    void Start()
    {
        // Broadcast once after other scripts had a chance to subscribe.
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    // --- API ---

    public void TakeDamage(int amount = 1)
    {
        if (IsDead) return;
        if (Time.time < invincibleUntil) return;

        currentHP = Mathf.Max(0, currentHP - Mathf.Abs(amount));
        invincibleUntil = Time.time + invincibleDuration;

        onDamaged?.Invoke();
        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP == 0) Die();
    }

    public void Heal(int amount = 1)
    {
        if (IsDead) return;
        currentHP = Mathf.Min(maxHP, currentHP + Mathf.Abs(amount));
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    // --- internals ---

    void Die()
    {
        if (IsDead) return;
        IsDead = true;

        if (disableOnDeath != null)
            foreach (var m in disableOnDeath)
                if (m) m.enabled = false;

        onDeath?.Invoke();
    }

    void SnapToValidValues(bool editMode)
    {
        maxHP = Mathf.Max(1, maxHP);

        // In edit mode show full HP; in play ensure valid start value.
        if (editMode || currentHP <= 0 || currentHP > maxHP)
            currentHP = maxHP;
    }
}
