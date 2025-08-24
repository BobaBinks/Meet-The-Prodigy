using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Rat : MonoBehaviour, IDrumAffectable, IGuitarAffectable
{
    Rigidbody2D rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void ApplyDrumEffect(float force, Vector2 direction)
    {
        if (!rigidBody)
            return;

        rigidBody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void ApplyGuitarEffect(float force, Vector2 direction)
    {
        if (!rigidBody)
            return;

        rigidBody.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
