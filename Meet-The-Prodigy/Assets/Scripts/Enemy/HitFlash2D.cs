using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class HitFlash2D : MonoBehaviour
{
    public Color flashColor = new Color(1f, 0.3f, 0.3f);
    public float flashTime = 0.12f;
    public float knockbackForce = 5f; // 0 = no knockback

    SpriteRenderer sr;
    Color originalColor;
    bool flashing;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>(); // auto-grab on same object
        originalColor = sr.color;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDamage += OnPlayerDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDamage -= OnPlayerDamage;
    }

    private void OnPlayerDamage()
    {
        Hit();
    }

    public void Hit(Vector2? knockbackDir = null, Rigidbody2D rb = null)
    {
        if (!flashing && sr) StartCoroutine(DoFlash());

        if (rb && knockbackForce > 0f && knockbackDir.HasValue)
            rb.AddForce(knockbackDir.Value.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    IEnumerator DoFlash()
    {
        flashing = true;
        sr.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        sr.color = originalColor;
        flashing = false;
    }
}
