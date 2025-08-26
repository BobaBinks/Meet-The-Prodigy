using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageOnTouch : MonoBehaviour
{
    public int damage = 1;

    public string requiredTag = "Player";

    // Solid collision setup
    private void OnCollisionEnter2D(Collision2D c) => TryDamage(c.collider);

    private void TryDamage(Collider2D other)
    {
        // Only count hits on player
        if (!other.CompareTag(requiredTag))
        {
            return;
        }

        var root = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        var hp = root.GetComponent<PlayerHealth>() ?? root.GetComponentInParent<PlayerHealth>();
        if (hp == null || hp.IsDead) return;

        hp.TakeDamage(damage);
    }
}
