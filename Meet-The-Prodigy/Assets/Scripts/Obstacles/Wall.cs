using UnityEngine;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer),typeof(Collider2D))]
public class Wall : MonoBehaviour, IGuitarAffectable
{
    [SerializeField] string wallCrumbleAnimationName;
    [SerializeField] Collider2D destroyedWallCollider;
    Animator _animator;
    Collider2D _collider2D;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void ApplyGuitarEffect(float force, Vector2 direction)
    {
        if (!_animator || !_collider2D)
            return;

        _animator.Play(wallCrumbleAnimationName);

        // crumble wall
        _collider2D.enabled = false;

        if(destroyedWallCollider)
            destroyedWallCollider.gameObject.SetActive(true);

        if(SoundManager.Instance && SoundLibrary.Instance)
            SoundManager.Instance.PlaySoundEffect(
                SoundLibrary.Instance.GetAudioClip(SoundLibrary.Obstacle.WALL_CRUMBLE));
    }
}
