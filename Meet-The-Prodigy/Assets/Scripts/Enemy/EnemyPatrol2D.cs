using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyPatrol2D : MonoBehaviour
{
    [Header("Patrol (no points/children needed)")]
    [Min(0.1f)] public float halfDistance = 3f;
    public float patrolSpeed = 2f;
    public bool startMovingRight = true;

    [Header("Visuals")]
    [Tooltip("If art faces RIGHT when flipX=false, keep true. If it faces LEFT by default, set false.")]
    public bool spriteFacesRight = true;
    public string speedParam = "speed";   // leave blank if you don't use it

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    float leftX, rightX;
    bool movingRight;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();     // required above
        anim = GetComponent<Animator>();           // optional

        float startX = transform.position.x;
        leftX = startX - halfDistance;
        rightX = startX + halfDistance;
        movingRight = startMovingRight;
    }

    void FixedUpdate()
    {
        float targetX = movingRight ? rightX : leftX;
        float dx = targetX - rb.position.x;
        float vx = Mathf.Sign(dx) * patrolSpeed;

        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);

        // Face travel direction
        bool goingRight = rb.linearVelocity.x > 0.01f;
        sr.flipX = spriteFacesRight ? !goingRight : goingRight;

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
        yield return new WaitForSeconds(seconds);
        rb.linearVelocity = old;
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
