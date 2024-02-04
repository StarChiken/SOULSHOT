using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OldPlayerScript : MonoBehaviour
{
    [Header("Basic Movement")]
    public float maxMoveSpeed;
    public float moveAccelerationTime;
    public float moveDecelerationTime;
    public float moveAccelerationMaxDampSpeed;
    public float moveDecelerationMaxDampSpeed;

    [Space(10)]
    [Header("Jump")]
    public float jumpForce;
    public float lowJumpGravScale;
    public float coyoteTime;
    public float jumpInputBufferTime;
    public float edgeBumpAllowanceOffset;

    [Space(10)]
    [Header("Crouch")]
    public float crouchMaxMoveSpeed;
    public float crouchMoveAccelerationTime;
    public float crouchMoveDecelerationTime;
    public float crouchTime;
    public float uncrouchTime;
    public CapsuleCollider2D crouchCollider;

    [Space(10)]
    [Header("Dash")]
    public float dashForce;
    public float dashMaxDampSpeed;

    [Space(10)]
    [Header("Animation")]
    public Sprite[] runSprites;
    public float[] runSpriteMaxVelocityRatio;
    public Sprite[] stopSprites;
    public Sprite[] crouchSprites;

    private bool isGrounded = false;
    private bool isJumping = false;

    private bool canStartCoyoteTimer = false;
    private bool inCoyoteTime = false;

    private bool jumpBuffered = false;
    private bool isInputtingJump = false;

    private bool isCrouching = false;
    private bool isUncrouching = false;

    private float moveDir;
    private float movementLerpTimeElapsed = 0;
    private float movementVelocityXStartingValue = 0;
    private float crouchLerpTimeElapsed = 0;

    private Sprite startingSprite;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private CircleCollider2D standingCollider;

    PlayerInput playerInput;
    InputAction horizontalInput;
    InputAction jumpInput;
    InputAction downInput;
    InputAction gamepadDownInput;
    InputAction dashInput;

    void Start()
    {
        AssignVaraibles();
        SetInputEvents();
    }

    private void AssignVaraibles()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingSprite = spriteRenderer.sprite;
        rb = GetComponent<Rigidbody2D>();
        standingCollider = GetComponent<CircleCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        horizontalInput = playerInput.actions["Horizontal"];
        jumpInput = playerInput.actions["Jump"];
        downInput = playerInput.actions["Down"];
        gamepadDownInput = playerInput.actions["Gamepad Down"];
        dashInput = playerInput.actions["Dash"];
    }

    private void SetInputEvents()
    {
        horizontalInput.performed += HorizontalInput;

        jumpInput.started += JumpInputStarted;
        jumpInput.canceled += JumpInputCanceled;

        downInput.started += DownInputStarted;
        downInput.canceled += DownInputCanceled;

        gamepadDownInput.performed += GamePadDownInput;

        dashInput.started += DashInputStarted;
    }

    void Update()
    {
        Crouching();

        HandleHorizontalMovementAnimation();
    }

    private void Crouching()
    {
        if (isCrouching)
        {
            HandleCrouchLerp();
        }
        else if (isUncrouching)
        {
            HandleUncrouchLerp();
        }
    }

    private void HandleCrouchLerp()
    {
        if (crouchLerpTimeElapsed < crouchTime)
        {
            crouchLerpTimeElapsed += Time.deltaTime;

            float crouchAnimationRatio = crouchLerpTimeElapsed / crouchTime;
            for (int i = 4; i > 0; i--)
            {
                if (crouchAnimationRatio > i * 0.18f)
                {
                    spriteRenderer.sprite = crouchSprites[i];
                    break;
                }
            }
        }
        else
        {
            standingCollider.enabled = false;
            crouchCollider.enabled = true;
            spriteRenderer.sprite = crouchSprites[5];
            crouchLerpTimeElapsed = crouchTime;
        }
    }

    private void HandleUncrouchLerp()
    {
        if (crouchLerpTimeElapsed < uncrouchTime)
        {
            crouchLerpTimeElapsed += Time.deltaTime;
            
            float crouchAnimationRatio = crouchLerpTimeElapsed / uncrouchTime;
            for (int i = 1; i < 4; i++)
            {
                if (crouchAnimationRatio < i * 0.25f)
                {
                    spriteRenderer.sprite = crouchSprites[4 - i];
                    break;
                }
            }
        }
        else
        {
            isUncrouching = false;
            standingCollider.enabled = true;
            crouchCollider.enabled = false;
            spriteRenderer.sprite = startingSprite;
            crouchLerpTimeElapsed = uncrouchTime;
        }
    }

    private void HandleHorizontalMovementAnimation()
    {
        if (isCrouching)
        {

        }
        else if (!isUncrouching)
        {
            float velMaxRatio = Mathf.Abs(rb.velocityX) / maxMoveSpeed;
            if (velMaxRatio >= runSpriteMaxVelocityRatio[3])
            {
                spriteRenderer.sprite = runSprites[3];
            }
            else if (velMaxRatio >= runSpriteMaxVelocityRatio[2])
            {
                spriteRenderer.sprite = runSprites[2];
            }
            else if (velMaxRatio >= runSpriteMaxVelocityRatio[1])
            {
                spriteRenderer.sprite = runSprites[1];
            }
            else if (velMaxRatio >= runSpriteMaxVelocityRatio[0])
            {
                spriteRenderer.sprite = runSprites[0];
            }
            else
            {
                spriteRenderer.sprite = startingSprite;
            }
        }

        if (Mathf.Sign(rb.velocityX) == -1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void StartCoyoteTimer()
    {
        if (canStartCoyoteTimer)
        {
            StartCoroutine(CoyoteTimer());
        }
    }

    public void GroundPlayer()
    {
        isGrounded = true;
        isJumping = false;
        canStartCoyoteTimer = true;
        rb.gravityScale = 1;

        if (jumpBuffered)
        {
            if (!isInputtingJump)
            {
                rb.gravityScale = lowJumpGravScale;
                jumpBuffered = false;
            }
            Jump();
        }
    }

    void FixedUpdate()
    {
        BasicHorizontalInput();
    }

    private void BasicHorizontalInput()
    {
        if (moveDir != 0)
        {
            AccelerateVelocityX();
        }
        else
        {
            DecelerateVelocityX();
        }
    }

    float velocityX = 0;

    private void AccelerateVelocityX()
    {
        if (isCrouching)
        {
            if (movementLerpTimeElapsed < crouchMoveAccelerationTime)
            {
                movementLerpTimeElapsed += Time.fixedDeltaTime;
            }
            else
            {
                movementLerpTimeElapsed = crouchMoveAccelerationTime;
            }
            rb.velocityX = Mathf.Lerp(movementVelocityXStartingValue, moveDir * crouchMaxMoveSpeed, movementLerpTimeElapsed / crouchMoveAccelerationTime);
        }
        else
        {
            if (movementLerpTimeElapsed < moveAccelerationTime)
            {
                movementLerpTimeElapsed += Time.fixedDeltaTime;
            }
            else
            {
                movementLerpTimeElapsed = moveAccelerationTime;
            }

            if (velocityX > maxMoveSpeed)
            {
                rb.velocityX = Mathf.SmoothDamp(rb.velocityX, moveDir * maxMoveSpeed, ref velocityX, moveAccelerationTime, dashMaxDampSpeed, Time.fixedDeltaTime);
            }
            else
            {
                rb.velocityX = Mathf.SmoothDamp(rb.velocityX, moveDir * maxMoveSpeed, ref velocityX, moveAccelerationTime, moveAccelerationMaxDampSpeed, Time.fixedDeltaTime);
            }
            //rb.velocityX = Mathf.Lerp(movementVelocityXStartingValue, moveDir * maxMoveSpeed, movementLerpTimeElapsed / moveAccelerationTime);
        }
    }

    private void DecelerateVelocityX()
    {
        if (isCrouching)
        {
            if (movementLerpTimeElapsed < crouchMoveDecelerationTime)
            {
                movementLerpTimeElapsed += Time.fixedDeltaTime;
            }
            else
            {
                movementLerpTimeElapsed = crouchMoveDecelerationTime;
            }
            rb.velocityX = Mathf.Lerp(movementVelocityXStartingValue, moveDir * crouchMaxMoveSpeed, movementLerpTimeElapsed / crouchMoveDecelerationTime);
        }
        else
        {
            if (movementLerpTimeElapsed < moveDecelerationTime)
            {
                movementLerpTimeElapsed += Time.fixedDeltaTime;
            }
            else
            {
                movementLerpTimeElapsed = moveDecelerationTime;
            }

            if (velocityX > maxMoveSpeed)
            {
                rb.velocityX = Mathf.SmoothDamp(rb.velocityX, moveDir * maxMoveSpeed, ref velocityX, moveDecelerationTime, dashMaxDampSpeed, Time.fixedDeltaTime);
            }
            else
            {
                rb.velocityX = Mathf.SmoothDamp(rb.velocityX, moveDir * maxMoveSpeed, ref velocityX, moveDecelerationTime, moveDecelerationMaxDampSpeed, Time.fixedDeltaTime);
            }
            //rb.velocityX = Mathf.Lerp(movementVelocityXStartingValue, moveDir * maxMoveSpeed, movementLerpTimeElapsed / moveDecelerationTime);
        }
    }

    private void HorizontalInput(InputAction.CallbackContext context)
    {
        if (moveDir != context.ReadValue<float>())
        {
            movementLerpTimeElapsed = 0;
            movementVelocityXStartingValue = rb.velocityX;
            moveDir = context.ReadValue<float>();
        }
    }

    private void JumpInputStarted(InputAction.CallbackContext context)
    {
        isInputtingJump = true;
        rb.gravityScale = 1;
        if (isGrounded || inCoyoteTime)
        {
            Jump();
        }
        else if (!jumpBuffered)
        {
            StartCoroutine(JumpBufferTimer());
        }
    }

    private void Jump()
    {
        if (!isCrouching)
        {
            rb.velocityY = 0;
            rb.AddForce(jumpForce * 10 * Vector2.up);
            isJumping = true;
            isGrounded = false;
            canStartCoyoteTimer = false;
        }
    }

    private void JumpInputCanceled(InputAction.CallbackContext context)
    {
        isInputtingJump = false;
        if (isJumping)
        {
            rb.gravityScale = lowJumpGravScale;
        }
    }

    private void DownInputStarted(InputAction.CallbackContext context)
    {
        if (!isJumping)
        {
            isCrouching = true;
            crouchLerpTimeElapsed = 0;
        }
    }

    private void DownInputCanceled(InputAction.CallbackContext context)
    {
        CancelCrouch();
    }

    private void GamePadDownInput(InputAction.CallbackContext context)
    {
        float currentGamepadLStickY = context.ReadValue<float>();
        if (currentGamepadLStickY < 0 && !isJumping && !isCrouching)
        {
            isCrouching = true;
            crouchLerpTimeElapsed = 0;
        }
        else if (currentGamepadLStickY > -0.05 && isCrouching)
        {
            CancelCrouch();
        }
    }

    private void CancelCrouch()
    {
        isUncrouching = true;
        isCrouching = false;
        crouchLerpTimeElapsed = 0;
        movementVelocityXStartingValue = rb.velocityX;
    }

    private void DashInputStarted(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector2.right * dashForce * moveDir, ForceMode2D.Impulse);
        movementLerpTimeElapsed = 0;
        movementVelocityXStartingValue = rb.velocityX;
    }

    IEnumerator CoyoteTimer()
    {
        canStartCoyoteTimer = false;
        inCoyoteTime = true;
        yield return new WaitForSeconds(coyoteTime);
        inCoyoteTime = false;
    }

    IEnumerator JumpBufferTimer()
    {
        jumpBuffered = true;
        yield return new WaitForSeconds(jumpInputBufferTime);
        jumpBuffered = false;
    }
}
