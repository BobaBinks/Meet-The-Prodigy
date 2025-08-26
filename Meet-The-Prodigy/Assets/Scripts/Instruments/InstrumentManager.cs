using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InstrumentManager : MonoBehaviour
{
    public enum Instruments 
    { 
        DRUM,
        ELECTRIC_GUITAR,
        FLUTE
    }

    [SerializeField] List<InstrumentBase> instruments;
    [SerializeField] PlayerVisuals playerVisuals;
    private InstrumentBase currInstrument;

    private void Start()
    {
        InitInstruments();
    }

    /// <summary>
    /// Disables all instrument gameobjects then equips the first one in the enum.
    /// </summary>
    private void InitInstruments()
    {
        if (instruments == null)
            return;

        foreach (var instrument in instruments)
        {
            instrument.gameObject.SetActive(false);
        }

        SwitchInstrument((Instruments)0);
    }

    /// <summary>
    /// Plays the current instrument equipped.
    /// </summary>
    /// <param name="context"></param>
    public void Fire(InputAction.CallbackContext context)
    {
        if (!currInstrument)
            return;

        if (context.performed)
        {
            currInstrument.PlayBeat();

            if (!playerVisuals)
                return;

            playerVisuals.CurrAnimator?.Play(currInstrument.PlayAnimationName);
        }
    }

    /// <summary>
    /// Switch to current equipped instrument to drum instrument.
    /// </summary>
    /// <param name="context"></param>
    public void SwitchToDrum(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        SwitchInstrument(Instruments.DRUM);
    }

    /// <summary>
    /// Switch to current equipped instrument to electric guitar instrument.
    /// </summary>
    /// <param name="context"></param>
    public void SwitchToElectricGuitar(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        SwitchInstrument(Instruments.ELECTRIC_GUITAR);
    }

    /// <summary>
    /// Switch to current equipped instrument to flute instrument.
    /// </summary>
    /// <param name="context"></param>
    public void SwitchToFlute(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        SwitchInstrument(Instruments.FLUTE);
    }

    /// <summary>
    /// Switch instruments by enum value.
    /// Handles activation/deactivation and tracking of the current instrument.
    /// </summary>
    /// <param name="instrument"></param>
    public void SwitchInstrument(Instruments instrument)
    {
        int instrumentIndex = (int)instrument;
        if (instruments == null || instruments.Count <= instrumentIndex)
            return;

        // deactivate current equipped instrument
        if (currInstrument)
            currInstrument.gameObject.SetActive(false);

        // swap current equipped instrument
        currInstrument = instruments[instrumentIndex];

        // activate instrument
        currInstrument.gameObject.SetActive(true);

        // set the player sprite to hold the right instrument
        if (!playerVisuals)
            return;

        playerVisuals.SetCurrentInstrumentVisual(instrument);
    }
}
