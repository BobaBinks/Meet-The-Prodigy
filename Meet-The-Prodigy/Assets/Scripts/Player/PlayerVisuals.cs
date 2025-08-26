using UnityEngine;
using System.Collections.Generic;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] List<InstrumentVisual> instrumentVisualsList;
    InstrumentVisual currInstrumentVisual;
    Dictionary<InstrumentManager.Instruments, InstrumentVisual> instrumentVisuals;

    public Animator CurrAnimator => currInstrumentVisual?.Animator;
    public SpriteFlipper CurrSpriteFlipper => currInstrumentVisual?.SpriteFlipper;

    private void Start()
    {
        instrumentVisuals = new Dictionary<InstrumentManager.Instruments, InstrumentVisual>();

        // convert the list to dictionary
        foreach(var visual in instrumentVisualsList)
        {
            if (visual == null)
                continue;

            instrumentVisuals.Add(visual.Instrument, visual);
        }

        SetCurrentInstrumentVisual(InstrumentManager.Instruments.DRUM);
    }

    public void SetCurrentInstrumentVisual(InstrumentManager.Instruments instrument)
    {
        if (instrumentVisuals == null)
            return;

        DeactivateAllInstrumentVisualGO();

        if (instrumentVisuals.ContainsKey(instrument))
        {
            currInstrumentVisual = instrumentVisuals[instrument];
            currInstrumentVisual.gameObject.SetActive(true);
        }
    }

    private void DeactivateAllInstrumentVisualGO()
    {
        if (instrumentVisuals == null)
            return;

        foreach (var kvp in instrumentVisuals)
        {
            InstrumentVisual instrumentVisual = kvp.Value;
            instrumentVisual.gameObject.SetActive(false);
        }
    }
}
