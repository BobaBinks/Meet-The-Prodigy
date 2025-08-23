using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InstrumentManager : MonoBehaviour
{
    [SerializeField] List<InstrumentBase> instruments;
    private InstrumentBase currInstrument;

    private void Start()
    {
        if (instruments.Count > 0)
        {
            currInstrument = instruments[0];
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (!currInstrument)
            return;

        if (context.performed)
            currInstrument.PlayBeat();
    }
}
