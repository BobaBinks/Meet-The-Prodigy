using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteFlipper))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField][Range(0, 100)] float jumpPower = 5f;
    [SerializeField][Range(0, 100)] float moveSpeed = 5f;

    bool isGrounded = true;
    Vector2 moveInput;
    
    Animator animator;
    Rigidbody2D rigidBody;
    SpriteFlipper spriteFlipper;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteFlipper = GetComponent<SpriteFlipper>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveInput.sqrMagnitude > 0 && spriteFlipper)
            spriteFlipper.FlipByDirection(rigidBody.linearVelocity);
    }

    private void FixedUpdate()
    {
        if (!rigidBody)
            return;

        //rigidBody.AddForce(new Vector2(moveInput.x * moveSpeed, 0), ForceMode2D.Force);

        if(moveInput.sqrMagnitude > 0)
        {
            rigidBody.linearVelocity = new Vector2(moveInput.x * moveSpeed, rigidBody.linearVelocityY);
        }

        if (!animator)
            return;

        animator.SetFloat("xVelocity", Math.Abs(rigidBody.linearVelocity.x));
        animator.SetFloat("yVelocity", rigidBody.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isGrounded || !rigidBody)
            return;

        rigidBody.AddForceY(jumpPower, ForceMode2D.Impulse);
        isGrounded = false;

        if (!animator)
            return;

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
