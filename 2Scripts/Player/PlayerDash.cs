using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [Header("Human Form")]
    public float humanDashCoolDown;
    public float humanDashForce;

    [Header("Demon Form")]
    public float demonDashCoolDown;
    public float demonDashForce;

    private bool canDash = true;

    private float moveDir = 0;

    private Rigidbody2D rb;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // TODO: Use Inheritance? also on PlayerAirDash
    private void OnDash()
    {
        if (canDash && Mathf.Abs(moveDir) > 0.05f && !Player.Instance.GetIsCrouching() && Player.Instance.GetIsGrounded())
        {
            if (canDash)
            {
                if (Player.Instance.GetPlayerForm() == Form.Human)
                {
                    rb.AddForce(humanDashForce * Vector2.right * Mathf.Sign(moveDir), ForceMode2D.Impulse);
                    DashCDTimer(humanDashCoolDown);
                }
                else if (Player.Instance.GetPlayerForm() == Form.Demon)
                {
                    rb.AddForce(demonDashForce * Vector2.right * Mathf.Sign(moveDir), ForceMode2D.Impulse);
                    DashCDTimer(demonDashCoolDown);
                }
            }
            animator.SetTrigger("dashTrigger");
        }
    }

    // TODO: Use Inheritance? Also on WallJump and PlayerHorizontalMove
    private void OnHorizontal(InputValue inputValue)
    {
        moveDir = inputValue.Get<float>();
    }

    // TODO: Use Inheritance? Also on WallJump. Inconsistent Naming
    private async void DashCDTimer(float dashCoolDown)
    {
        canDash = false;
        await Awaitable.WaitForSecondsAsync(dashCoolDown);
        canDash = true;
    }
}
