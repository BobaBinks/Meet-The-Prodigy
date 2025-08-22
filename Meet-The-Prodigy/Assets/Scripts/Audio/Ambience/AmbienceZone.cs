using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Collider2D))]
public class AmbienceZone : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] AudioSource audioSource;

    [Tooltip("Maximum distance from the zone's collider at which the ambience is still audible. " +
        "At this distance the volume fades to 0; closer distances fade up to full volume.")]
    [SerializeField] float fadeDistance = 5f;
    Collider2D collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FadeAmbienceByDistance();
    }

    /// <summary>
    /// Adjusts the audio source volume based on how close the player is.
    /// </summary>
    private void FadeAmbienceByDistance()
    {
        if (!audioSource || fadeDistance <= 0)
            return;

        float distanceToPlayer = GetDistanceToPlayer(player);

        if (float.IsNaN(distanceToPlayer))
            return;

        float volume = 1f - Mathf.Clamp01(distanceToPlayer / fadeDistance);
        audioSource.volume = volume;
    }

    /// <summary>
    /// Returns the shortest distance between the player and this zone's collider.
    /// </summary>
    private float GetDistanceToPlayer(GameObject player)
    {
        if (!player)
        {
            Debug.Log($"AmbienceZone {name}: GetDistanceToPlayer could not find player object.");
            return float.NaN;
        }

        if (!collider)
        {
            Debug.Log($"AmbienceZone {name}: does not have a collider.");
            return float.NaN;
        }

        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 closestPoint = collider.ClosestPoint(playerPosition);

        return (closestPoint - playerPosition).magnitude;
    }
}
