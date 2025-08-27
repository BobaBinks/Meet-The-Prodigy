using UnityEngine;

public class GateFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameTimer.Instance != null)
                GameTimer.Instance.StopAndGrade();
        }
    }
}
