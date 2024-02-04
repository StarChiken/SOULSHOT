using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallJump : MonoBehaviour
{
    public float wallJumpForce;
    public float wallJumpWallRepelForce;
    public float jumpInputBufferTime;
    public float rayXOffset;
    public float rayYOffset;

    private bool jumpBuffered = false;
    private bool canWallJump = false;
    private bool isInputtingJump = false;

    private float moveDir;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player.onGroundedChange += OnGroundChange;
    }

    private void FixedUpdate()
    {
        if (isInputtingJump && jumpBuffered)
        {
            CheckForWallJump();
        }
    }

    private void OnJump(InputValue inputValue)
    {
        isInputtingJump = inputValue.isPressed;
        if (isInputtingJump && canWallJump)
        {
            CheckForWallJump();
        }
    }

    private void CheckForWallJump()
    {
        if (moveDir > 0)
        {
            RaycastHit2D rightHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(rayXOffset, rayYOffset), Vector2.right, 0.01f);
            if (rightHit.transform != null && rightHit.transform.tag == "Ground")
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(-wallJumpWallRepelForce, wallJumpForce), ForceMode2D.Impulse);
                WallJumpCD();
                print("Right Wall Jump");
            }
            else
            {
                JumpBufferTimer();
            }
        }
        else if (moveDir < 0)
        {
            RaycastHit2D leftHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(-rayXOffset, rayYOffset), Vector2.left, 0.01f);
            if (leftHit.transform != null && leftHit.transform.tag == "Ground")
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(wallJumpWallRepelForce, wallJumpForce), ForceMode2D.Impulse);
                WallJumpCD();
                print("Left Wall Jump");
            }
            else
            {
                JumpBufferTimer();
            }
        }
    }

    private void OnHorizontal(InputValue inputValue)
    {
        moveDir = inputValue.Get<float>();
    }

    private void OnGroundChange(bool isGrounded)
    {
        if (!isGrounded)
        {
            WallJumpCD();
        }
        else
        {
            canWallJump = false;
        }
    }

    private async void WallJumpCD()
    {
        canWallJump = false;
        await Awaitable.WaitForSecondsAsync(0.1f);
        canWallJump = true;
    }

    private async void JumpBufferTimer()
    {
        jumpBuffered = true;
        await Awaitable.WaitForSecondsAsync(jumpInputBufferTime);
        jumpBuffered = false;
    }
}
