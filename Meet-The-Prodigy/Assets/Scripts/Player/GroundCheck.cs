using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float extraHeight = 0.05f;
    [SerializeField] private LayerMask layerMask;
    BoxCollider2D boxCollider;
    Color debugColor;

    public bool isGrounded { get; private set; }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider.bounds.size, 0f, Vector2.down, extraHeight, layerMask);
        return hit.collider != null;
    }

    private void Update()
    {
        isGrounded = IsGrounded();

        debugColor = isGrounded ? Color.green : Color.red;
    }

    private void OnDrawGizmos()
    {
        if (!boxCollider) boxCollider = GetComponent<BoxCollider2D>();

        // Draw the cast area for debugging
        Gizmos.color = debugColor;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + Vector3.down * extraHeight,
            boxCollider.bounds.size);
    }
}
