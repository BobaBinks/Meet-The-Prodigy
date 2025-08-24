using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Car : MonoBehaviour, IGuitarAffectable
{
    Rigidbody2D rigidbody2D;

    private void Start()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyGuitarEffect(float force, Vector2 direction)
    {
        if (!rigidbody2D)
            return;

        rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
