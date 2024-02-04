using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchTime;
    public float uncrouchTime;

    private Animator animator;

    private CircleCollider2D circleCol;
    private CapsuleCollider2D capsuleCol;

    void Start()
    {
        animator = GetComponent<Animator>();

        circleCol = GetComponent<CircleCollider2D>();
        capsuleCol = GetComponent<CapsuleCollider2D>();
        capsuleCol.enabled = false;
    }

    private void OnDown(InputValue inputValue)
    {
        if (inputValue.isPressed && Player.Instance.GetIsGrounded())
        {
            SetDownValues(true);
        }
        else
        {
            SetDownValues(false);
        }
    }

    private void SetDownValues(bool downBool)
    {
        animator.SetBool("isCrouching", downBool);

        Player.Instance.SetIsCrouching(downBool);

        circleCol.enabled = !downBool;
        capsuleCol.enabled = downBool;
    }
}
