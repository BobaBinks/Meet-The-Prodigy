using System.Collections;
using UnityEngine;

public abstract class InstrumentBase : MonoBehaviour
{
    [Tooltip("Instrument Cone")]
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected string playAnimationName = "DEFUALT_PLAY_ANIMATION_NAME";

    [Header("Instrument Parameters")]
    [SerializeField] protected float coolDownTimer = 1f;
    protected bool onCooldown = false;

    [HideInInspector] public Vector2 lookDirection;

    public string PlayAnimationName => playAnimationName;
    public Collider2D Collider2D => _collider;

    [Header("Sound Manager"), Tooltip("For playing instrument sound effects")]
    [SerializeField] protected InstrumentSoundManager instrumentSoundManager;


    public virtual bool PlayBeat()
    {
        if (onCooldown)
            return false;

        // play instrument sfx
        if (instrumentSoundManager)
            instrumentSoundManager.PlaySound();

        // apply instrument effects on targets
        AffectInstrumentTargets();

        StartCoroutine(CoolDownCoroutine(coolDownTimer));

        return true;
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

    protected virtual IEnumerator CoolDownCoroutine(float coolDownTimer)
    {
        coolDownTimer = Mathf.Max(0, coolDownTimer);
        onCooldown = true;

        yield return new WaitForSeconds(coolDownTimer);

        onCooldown = false;
        
    }
}
