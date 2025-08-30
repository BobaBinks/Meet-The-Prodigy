using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Rat : MonoBehaviour, IDrumAffectable, IGuitarAffectable, IFluteAffectable
{
    Rigidbody2D rb;
    EnemyPatrol2D patrol;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        patrol = GetComponent<EnemyPatrol2D>();
    }

    // External force (e.g., drum/ability)
    public void ApplyDrumEffect(float force, Vector2 direction)
    {
        if (!rb) return;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        if (patrol) patrol.Pause(0.1f);
    }

    public void ApplyGuitarEffect(float force, Vector2 direction)
    {
        if (!rb)
            return;

        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void ApplySleepEffect(float sleepDuration)
    {
        if (!patrol)
            return;

        Debug.Log($"{name}'s asleep for {sleepDuration} seconds");
        patrol.Pause(sleepDuration);
    }
}
