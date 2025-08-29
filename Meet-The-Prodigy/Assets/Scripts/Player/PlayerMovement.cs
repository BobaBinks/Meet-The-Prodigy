using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerVisuals playerVisuals;
    [SerializeField] GroundCheck groundChecker;
    [SerializeField] PlayerInput playerInput;

    [Header("Movement Parameters")]
    [SerializeField][Range(0, 100)] float jumpPower = 5f;
    [SerializeField][Range(0, 100)] float moveSpeed = 5f;

    bool wasGrounded;
    Vector2 moveInput;
    
    Animator animator;
    Rigidbody2D rigidBody;

    Vector2 spriteDirection;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        SettingsManager.OnPause += OnPause;
    }

    private void OnDisable()
    {
        SettingsManager.OnPause -= OnPause;
    }

    private void OnPause(bool pause)
    {
        if (!playerInput)
            return;

        playerInput.enabled = !pause;
    }

    // Update is called once per frame
    void Update()
    {
        // get current sprite flipper and flip
        if(playerVisuals &&
           playerVisuals.CurrSpriteFlipper)
            playerVisuals.CurrSpriteFlipper.FlipByDirection(spriteDirection);

        if (!groundChecker)
            return;

        bool isGrounded = groundChecker.isGrounded;

        // was in the air but just landed
        if(!wasGrounded && isGrounded &&
            playerVisuals && playerVisuals.CurrAnimator)
        {
            playerVisuals.CurrAnimator.SetTrigger("land");
            PlayLandingSound();
        }

        wasGrounded = isGrounded;
    }

    private void FixedUpdate()
    {
        if (!rigidBody)
            return;

        //rigidBody.AddForce(new Vector2(moveInput.x * moveSpeed, 0), ForceMode2D.Force);
        spriteDirection = rigidBody.linearVelocity;
        if (moveInput.sqrMagnitude > 0)
        {
            rigidBody.linearVelocity = new Vector2(moveInput.x * moveSpeed, rigidBody.linearVelocityY);
        }

        // if cant find player animator, return
        if (!playerVisuals && playerVisuals.CurrAnimator)
            return;

        playerVisuals.CurrAnimator.SetFloat("xVelocity", Math.Abs(rigidBody.linearVelocity.x));

        playerVisuals.CurrAnimator.SetFloat("yVelocity", rigidBody.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (groundChecker && !groundChecker.isGrounded || !rigidBody)
            return;

        if (context.performed)
        {
            playerVisuals.CurrAnimator.SetTrigger("jump");
            rigidBody.AddForceY(jumpPower, ForceMode2D.Impulse);
            PlayJumpStartSound();
        }
    }

    private void PlayJumpStartSound()
    {
        // sound effect
        if (!SoundManager.Instance || !SoundLibrary.Instance)
            return;

        // retrieve the audio clip
        AudioClip clip = SoundLibrary.Instance.GetAudioClip(SoundLibrary.Player.JUMP_START_1);
        SoundManager.Instance.PlaySoundEffect(clip);
    }

    private void PlayLandingSound()
    {
        // sound effect
        if (!SoundManager.Instance || !SoundLibrary.Instance)
            return;

        // retrieve the audio clip
        AudioClip clip = SoundLibrary.Instance.GetAudioClip(SoundLibrary.Player.JUMP_LAND_1);
        SoundManager.Instance.PlaySoundEffect(clip);
    }


}
