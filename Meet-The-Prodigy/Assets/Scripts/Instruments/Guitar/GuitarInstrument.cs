using UnityEngine;
using System.Collections.Generic;

public class GuitarInstrument : InstrumentBase
{
    [SerializeField] float force = 10f;
    [SerializeField] Collider2D _collider;

    Vector2 lookDirection;

    HashSet<IGuitarAffectable> guitarTargets;

    private void Start()
    {
        guitarTargets = new HashSet<IGuitarAffectable>();
    }

    private void Update()
    {
        // get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        lookDirection = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        _collider.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void AffectInstrumentTargets()
    {
        if (guitarTargets == null)
            return;

        // apply affects to all affectable targets
        foreach (var target in guitarTargets)
        {
            target.ApplyGuitarEffect(force, lookDirection);
        }
    }

    public override void AddInstrumentTarget(Collider2D collider)
    {
        if (guitarTargets == null || !collider)
            return;

        // adds the object to drum targets if it contains the drum affectable component / interface.
        if (collider.TryGetComponent<IGuitarAffectable>(out IGuitarAffectable target))
        {
            guitarTargets.Add(target);
        }
    }

    public override void RemoveInstrumentTarget(Collider2D collider)
    {
        if (guitarTargets == null || !collider)
            return;

        // adds the object to drum targets if it contains the drum affectable component / interface.
        if (collider.TryGetComponent<IGuitarAffectable>(out IGuitarAffectable target))
        {
            guitarTargets.Remove(target);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = new Vector3(lookDirection.x, lookDirection.y);
        Debug.DrawLine(transform.position, transform.position + dir * 3, Color.red);

    }
}
