using UnityEngine;
using System.Collections.Generic;


public class FluteInstrument : InstrumentBase
{
    [SerializeField] float sleepDuration = 3f;

    [SerializeField] Collider2D _collider;
    HashSet<IFluteAffectable> fluteTargets;

    Vector2 lookDirection;

    void Start()
    {
        fluteTargets = new HashSet<IFluteAffectable>();
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
        if (fluteTargets == null)
            return;

        // ensure sleep duration not less than 0.1f
        sleepDuration = Mathf.Max(sleepDuration, 0.1f);

        // apply affects to all affectable targets
        foreach (var target in fluteTargets)
        {
            target.ApplySleepEffect(sleepDuration);
        }
    }

    public override void AddInstrumentTarget(Collider2D collider)
    {
        if (!collider || fluteTargets == null)
            return;

        // adds the object to drum targets if it contains the drum affectable component / interface.
        if (collider.TryGetComponent<IFluteAffectable>(out IFluteAffectable target))
        {
            fluteTargets.Add(target);
        }
    }

    public override void RemoveInstrumentTarget(Collider2D collider)
    {
        if (fluteTargets == null || !collider)
            return;

        // adds the object to drum targets if it contains the drum affectable component / interface.
        if (collider.TryGetComponent<IFluteAffectable>(out IFluteAffectable target))
        {
            fluteTargets.Remove(target);
        }
    }
}
