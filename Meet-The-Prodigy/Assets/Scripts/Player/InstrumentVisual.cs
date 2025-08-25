using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator),typeof(SpriteFlipper))]
public class InstrumentVisual : MonoBehaviour
{
    [SerializeField] InstrumentManager.Instruments instrument;

    public InstrumentManager.Instruments Instrument => instrument;

    SpriteRenderer spriteRenderer;
    SpriteFlipper spriteFlipper;
    Animator animator;

    public Animator Animator => animator;
    public SpriteFlipper SpriteFlipper => spriteFlipper;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteFlipper = GetComponent<SpriteFlipper>();
        animator = GetComponent<Animator>();
    }

    public void OnStep()
    {
        // sound effect
        if (!SoundManager.Instance || !SoundLibrary.Instance)
            return;

        // get random footstep
        int clipIndex = UnityEngine.Random.Range((int)SoundLibrary.Player.FOOTSTEP_1, (int)SoundLibrary.Player.FOOTSTEP_5 + 1);

        // convert back int back to enum
        SoundLibrary.Player footStep = (SoundLibrary.Player)clipIndex;

        // retrieve the audio clip
        AudioClip clip = SoundLibrary.Instance.GetAudioClip(footStep);

        SoundManager.Instance.PlaySoundEffect(clip);
    }
}
