using UnityEngine;
using System.Collections.Generic;

public class DrumInstrument : InstrumentBase
{
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] float force = 10f;
    [SerializeField] Collider2D collider;

    HashSet<IDrumAffectable> drumTargets;

    Vector2 lookDirection;

    private void Start()
    {
        drumTargets = new HashSet<IDrumAffectable>();
    }

    private void Update()
    {
        // get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        lookDirection = (mousePos - transform.position).normalized;

        Debug.Log($"Look Direction: {lookDirection}");

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        collider.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void PlayBeat()
    {
        // apply force to player in the opposite direction
        if (playerRB)
            playerRB.AddForce(-lookDirection * force, ForceMode2D.Impulse);

        // play instrument sfx
        if (instrumentSoundManager)
            instrumentSoundManager.PlaySound();

        if (drumTargets == null)
            return;

        // apply affects to all affectable targets
        foreach(var target in drumTargets)
        {
            target.ApplyDrumEffect(force, lookDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (drumTargets == null)
            return;

        // adds the object to drum targets if it contains the drum affectable component / interface.
        if(collision.TryGetComponent<IDrumAffectable>(out IDrumAffectable target))
        {
            drumTargets.Add(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (drumTargets == null)
            return;

        // adds the object to drum targets if it contains the drum affectable component / interface.
        if (collision.TryGetComponent<IDrumAffectable>(out IDrumAffectable target))
        {
            drumTargets.Remove(target);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = new Vector3(lookDirection.x, lookDirection.y);
        Debug.DrawLine(transform.position, transform.position + dir * 3, Color.red);

    }
}
