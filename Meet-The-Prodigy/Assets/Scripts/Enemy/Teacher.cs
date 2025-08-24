using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Teacher : MonoBehaviour, IDrumAffectable
{
    Rigidbody2D rb;
    EnemyPatrol2D patrol;
    HitFlash2D flash;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        patrol = GetComponent<EnemyPatrol2D>();
        flash = GetComponent<HitFlash2D>();
    }

    public void ApplyDrumEffect(float force, Vector2 direction)
    {
        if (!rb) return;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        if (flash) flash.Hit(-direction, rb);
        if (patrol) patrol.Pause(0.1f);
    }
}
