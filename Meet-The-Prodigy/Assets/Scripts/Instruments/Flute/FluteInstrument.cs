using UnityEngine;
using System.Collections.Generic;


public class FluteInstrument : InstrumentBase
{
    [SerializeField] float sleepDuration = 3f;
    HashSet<IFluteAffectable> fluteTargets;

    Vector2 lookDirection;

    void Start()
    {
        fluteTargets = new HashSet<IFluteAffectable>();
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
