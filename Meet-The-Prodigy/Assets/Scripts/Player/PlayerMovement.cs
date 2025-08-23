using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteFlipper))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField][Range(0, 5)] float jumpPower = 5f;
    [SerializeField][Range(0, 5)] float moveSpeed = 5f;

    bool isGrounded = true;
    Vector2 moveInput;
    

    Rigidbody2D rb;
    Animator animator;
    SpriteFlipper spriteFlipper;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteFlipper = GetComponent<SpriteFlipper>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveInput.sqrMagnitude > 0)
            spriteFlipper.FlipByDirection(rb.linearVelocity);

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY);

        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isGrounded)
            return;

        rb.AddForceY(jumpPower, ForceMode2D.Impulse);
        isGrounded = false;
        animator.SetBool("isJumping", !isGrounded);

        // sound effect
        if (!SoundManager.Instance || !SoundLibrary.Instance)
            return;

        // retrieve the audio clip
        AudioClip clip = SoundLibrary.Instance.GetAudioClip(SoundLibrary.Player.JUMP_START_1);
        SoundManager.Instance.PlaySoundEffect(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);

        // sound effect
        if (!SoundManager.Instance || !SoundLibrary.Instance)
            return;

        // retrieve the audio clip
        AudioClip clip = SoundLibrary.Instance.GetAudioClip(SoundLibrary.Player.JUMP_LAND_1);
        SoundManager.Instance.PlaySoundEffect(clip);
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
