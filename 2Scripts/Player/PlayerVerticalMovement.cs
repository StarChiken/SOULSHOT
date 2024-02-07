using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVerticalMovement : MonoBehaviour
{
    [Header("Jump")]
    public float humanJumpForce;
    public float demonJumpForce;
    public float jumpInputBufferTime;
    public float jumpBufferjumpCutBufferTime;
    public float coyoteTime;

    [Header("Increased Fall Gravity")]
    public float velGravChange;
    public float fallGrav;

    [Header("Jump Cut")]
    public float lowJumpGravScale;
    [Range(0, 1)]
    public float velCutMult;

    [Header("Toggles")]
    public bool enableJumpCutting = false;
    public bool enableIncreasedFallGravity = false;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool jumpBuffered = false;
    private bool canStartCoyoteTimer = false;
    private bool inCoyoteTime = false;
    private bool isInputtingJump = false;
    private bool canJumpCut = true;

    private Rigidbody2D rb;

    private Form playerForm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Player.onFormChange += OnFormChange;
        Player.onGroundedChange += OnGroundedChange;
    }

    void FixedUpdate()
    {
        if (enableIncreasedFallGravity && rb.velocityY < velGravChange)
        {
            rb.gravityScale = fallGrav;
            canJumpCut = false;
        }
    }

    private void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            JumpInputStarted();
        }
        else
        {
            JumpInputCanceled();
        }
    }

    private void JumpInputStarted()
    {
        isInputtingJump = true;

        if (!Player.Instance.GetIsCrouching())
        {
            if (isGrounded || inCoyoteTime)
            {
                Jump();
            }
            else if (!jumpBuffered)
            {
                JumpBufferTimer();
            }
        }
    }

    private void Jump()
    {
        rb.gravityScale = 1;
        rb.velocityY = 0;

        if (playerForm == Form.Human)
        {
            rb.AddForce(humanJumpForce * Vector2.up, ForceMode2D.Impulse);
        }
        else if (playerForm == Form.Demon)
        {
            rb.AddForce(demonJumpForce * Vector2.up, ForceMode2D.Impulse);
        }

        isJumping = true;
        canStartCoyoteTimer = false;
        canJumpCut = true;

        Player.Instance.SetIsGrounded(false);
    }

    private void JumpInputCanceled()
    {
        isInputtingJump = false;

        if (isJumping && canJumpCut)
        {
            if (enableJumpCutting)
            {
                CutJump();
            }
            canJumpCut = false;
        }
    }

    // TODO: Can be made private. Later use???
    public void GroundPlayer()
    {
        isJumping = false;
        canStartCoyoteTimer = true;
        canJumpCut = false;
        rb.gravityScale = 1;

        if (jumpBuffered)
        {
            if (!isInputtingJump)
            {
                jumpBuffered = false;
            }

            Jump();

            canJumpCut = true;

            if (!isInputtingJump && canJumpCut && enableJumpCutting)
            {
                BufferedJumpCutTimer();
            }
        }
    }

    // TODO: Can be made private. Later use???
    public void CutJump()
    {
        rb.velocityY *= velCutMult;
        rb.gravityScale = lowJumpGravScale;
    }

    private async void CoyoteTimer()
    {
        canStartCoyoteTimer = false;
        inCoyoteTime = true;
        await Awaitable.WaitForSecondsAsync(coyoteTime);
        inCoyoteTime = false;
    }

    private async void JumpBufferTimer()
    {
        jumpBuffered = true;
        await Awaitable.WaitForSecondsAsync(jumpInputBufferTime);
        jumpBuffered = false;
    }

    private async void BufferedJumpCutTimer()
    {
        await Awaitable.WaitForSecondsAsync(jumpBufferjumpCutBufferTime);
        CutJump();
        canJumpCut = false;
    }

    private void OnFormChange(Form form)
    {
        playerForm = form;
    }

    private void OnGroundedChange(bool _isGrounded)
    {
        isGrounded = _isGrounded;
        if (_isGrounded)
        {
            GroundPlayer();
        }
        else if (canStartCoyoteTimer && !inCoyoteTime)
        {
            CoyoteTimer();
        }
    }
}
