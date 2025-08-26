using UnityEngine;

public abstract class InstrumentBase : MonoBehaviour
{
    [Header("Sound Manager"), Tooltip("For playing instrument sound effects")]
    [SerializeField] protected InstrumentSoundManager instrumentSoundManager;

    public virtual void PlayBeat()
    {
        // play instrument sfx
        if (instrumentSoundManager)
            instrumentSoundManager.PlaySound();

        // apply instrument effects on targets
        AffectInstrumentTargets();
    }
    public abstract void AffectInstrumentTargets();
    public abstract void AddInstrumentTarget(Collider2D collider);
    public abstract void RemoveInstrumentTarget(Collider2D collider);

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        AddInstrumentTarget(collider: collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveInstrumentTarget(collider: collision);
    }
}
