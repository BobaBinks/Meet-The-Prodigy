using UnityEngine;

public abstract class InstrumentBase : MonoBehaviour
{
    [Header("Sound Manager"), Tooltip("For playing instrument sound effects")]
    [SerializeField] protected InstrumentSoundManager instrumentSoundManager;

    public abstract void PlayBeat();
}
