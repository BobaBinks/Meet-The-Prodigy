using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteFlipper))]
[RequireComponent(typeof(DamageOnTouch))]
public class EnemyPatrol2D : MonoBehaviour
{
    [Header("Patrol")]
    [Min(0.1f)] public float halfDistance = 3f;
    public float patrolSpeed = 2f;
    public bool startMovingRight = true;

    [Header("Visuals")]
    public string speedParam = "speed";
    public string pauseParam = "isPaused";

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    SpriteFlipper spriteFlipper;
    DamageOnTouch damageOnTouchComponent;

    float leftX, rightX;
    bool movingRight;
    bool isPaused = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        spriteFlipper = GetComponent<SpriteFlipper>();
        damageOnTouchComponent = GetComponent<DamageOnTouch>();

        float startX = transform.position.x;
        leftX = startX - halfDistance;
        rightX = startX + halfDistance;
        movingRight = startMovingRight;
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;

        float targetX = movingRight ? rightX : leftX;
        float dx = targetX - rb.position.x;
        float vx = Mathf.Sign(dx) * patrolSpeed;

        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

        // Face travel direction
        spriteFlipper.FlipByDirection(rb.linearVelocity);

        if (anim && !string.IsNullOrEmpty(speedParam))
            anim.SetFloat(speedParam, Mathf.Abs(rb.linearVelocity.x));

        const float eps = 0.05f;
        if (movingRight && rb.position.x >= rightX - eps) movingRight = false;
        else if (!movingRight && rb.position.x <= leftX + eps) movingRight = true;
    }

    public void Pause(float seconds) => StartCoroutine(PauseRoutine(seconds));
    IEnumerator PauseRoutine(float seconds)
    {
        var old = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
        isPaused = true;

        damageOnTouchComponent.enabled = false;

        if(anim && !string.IsNullOrEmpty(pauseParam))
            anim.SetBool(pauseParam, isPaused);

        yield return new WaitForSeconds(seconds);
        isPaused = false;
        rb.linearVelocity = old;
        damageOnTouchComponent.enabled = true;

        if (anim && !string.IsNullOrEmpty(pauseParam))
            anim.SetBool(pauseParam, isPaused);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        float startX = Application.isPlaying ? (leftX + rightX) * 0.5f : transform.position.x;
        float l = Application.isPlaying ? leftX : startX - halfDistance;
        float r = Application.isPlaying ? rightX : startX + halfDistance;
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.9f);
        Gizmos.DrawLine(new Vector3(l, transform.position.y, 0), new Vector3(r, transform.position.y, 0));
        Gizmos.DrawSphere(new Vector3(l, transform.position.y, 0), 0.07f);
        Gizmos.DrawSphere(new Vector3(r, transform.position.y, 0), 0.07f);
    }
#endif
}
