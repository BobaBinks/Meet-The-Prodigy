using System.Collections;
using UnityEngine;

public abstract class InstrumentSoundManager : MonoBehaviour
{
    protected int nextClipIndex = 0;
    protected float delay = 5f;

    protected Coroutine resetInstrumentClipIndexCoroutine;

    public abstract void PlaySound();

    protected virtual IEnumerator ResetIndexAfterDelay(float delay)
    {
        delay = Mathf.Max(delay, 0);
        yield return new WaitForSeconds(delay);
        nextClipIndex = 0;
        Debug.Log("Index reset");
    }

    protected virtual void ResetAfterDelay(float delay)
    {
        // reset coroutine every time sound is played
        if (resetInstrumentClipIndexCoroutine != null)
        {
            StopCoroutine(resetInstrumentClipIndexCoroutine);
        }
        resetInstrumentClipIndexCoroutine = StartCoroutine(ResetIndexAfterDelay(delay));
    }

    protected virtual void UpdateNextClipIndex(int length)
    {
        // update next Clip index to play
        nextClipIndex++;

        // wrap around index
        nextClipIndex = (int)Mathf.Repeat(nextClipIndex, length);
    }

    protected virtual void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    PlaySound();
        //}
    }
}
