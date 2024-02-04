using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontalMovement : MonoBehaviour
{
    public Human human;

    public Demon demon;

    private float moveDir = 0;

    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    private Form playerForm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Player.onFormChange += OnFormChange;
    }

    private void FixedUpdate()
    {
        float addVel = 0;
        if (!Player.Instance.GetIsGrounded())
        {
            addVel = MidAirMovement();
        }
        else if (!Player.Instance.GetIsCrouching())
        {
            addVel = GroundMovement();
        }
        else
        {
            addVel = CrouchMovement();
        }
        rb.AddForce(addVel * Vector2.right);

        if (rb.velocityX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private float GroundMovement()
    {
        float speedDif = 0;
        float movement = 0;
        if (Mathf.Abs(moveDir) < 0.05f)
        {
            speedDif = -rb.velocityX;
            if (playerForm == Form.Human)
            {
                movement = speedDif * human.decelerationRate * Time.fixedDeltaTime;
            }
            else if (playerForm == Form.Demon)
            {
                movement = speedDif * demon.decelerationRate * Time.fixedDeltaTime;
            }

            if (Mathf.Abs(rb.velocityX) < 0.05)
            {
                rb.velocityX = 0;
            }
        }
        else if (Mathf.Sign(moveDir) != Mathf.Sign(rb.velocityX))
        {
            if (playerForm == Form.Human)
            {
                speedDif = (human.targetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * human.turnAccelerationRate * Time.fixedDeltaTime;
            }
            else if (playerForm == Form.Demon)
            {
                speedDif = (demon.targetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * demon.turnAccelerationRate * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (playerForm == Form.Human)
            {
                speedDif = (human.targetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * human.accelerationRate * Time.fixedDeltaTime;
            }
            else if (playerForm == Form.Demon)
            {
                speedDif = (demon.targetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * demon.accelerationRate * Time.fixedDeltaTime;
            }
        }
        return movement;
    }

    private float MidAirMovement()
    {
        float speedDif = 0;
        float movement = 0;
        if (Mathf.Abs(moveDir) < 0.05f)
        {
            speedDif = -rb.velocityX;
            if (playerForm == Form.Human)
            {
                movement = speedDif * human.airDecelerationRate * Time.fixedDeltaTime;
            }
            else if (playerForm == Form.Demon)
            {
                movement = speedDif * demon.airDecelerationRate * Time.fixedDeltaTime;
            }

            if (Mathf.Abs(rb.velocityX) < 0.05)
            {
                rb.velocityX = 0;
            }
        }
        else if (Mathf.Sign(moveDir) != Mathf.Sign(rb.velocityX))
        {
            if (playerForm == Form.Human)
            {
                speedDif = (human.airTargetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * human.airTurnAccelerationRate * Time.fixedDeltaTime;
            }
            else if (playerForm == Form.Demon)
            {
                speedDif = (demon.targetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * demon.airTurnAccelerationRate * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (playerForm == Form.Human)
            {
                speedDif = (human.targetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * human.airAccelerationRate * Time.fixedDeltaTime;
            }
            else if (playerForm == Form.Demon)
            {
                speedDif = (demon.targetSpeed * moveDir) - rb.velocityX;
                movement = speedDif * demon.airAccelerationRate * Time.fixedDeltaTime;
            }
        }
        return movement;
    }
    private float CrouchMovement()
    {
        /*
        float speedDif = 0;
        float movement = 0;
        if (Mathf.Abs(moveDir) < 0.05f)
        {
            speedDif = -rb.velocityX;
            movement = speedDif * human.crouchDecelerationRate * Time.fixedDeltaTime;
            if (Mathf.Abs(rb.velocityX) < 0.05)
            {
                rb.velocityX = 0;
            }
        }
        else if (Mathf.Sign(moveDir) != Mathf.Sign(rb.velocityX))
        {
            speedDif = (crouchTargetSpeed * moveDir) - rb.velocityX;
            movement = speedDif * crouchTurnAccelerationRate * Time.fixedDeltaTime;
        }
        else
        {
            speedDif = (crouchTargetSpeed * moveDir) - rb.velocityX;
            movement = speedDif * crouchAccelerationRate * Time.fixedDeltaTime;
        }
        */
        return 0;
    }

    private void OnHorizontal(InputValue inputValue)
    {
        moveDir = inputValue.Get<float>();
    }

    private void OnFormChange(Form form)
    {
        playerForm = form;
    }
}

[Serializable]
public class Demon
{
    [Header("Demon Ground Move")]
    public float targetSpeed;
    public float accelerationRate;
    public float decelerationRate;
    public float turnAccelerationRate;
    [Space(10)]
    [Header("Demon Mid-Air Move")]
    public float airTargetSpeed;
    public float airAccelerationRate;
    public float airDecelerationRate;
    public float airTurnAccelerationRate;
}

[Serializable]
public class Human
{
    [Header("Human Ground Move")]
    public float targetSpeed;
    public float accelerationRate;
    public float decelerationRate;
    public float turnAccelerationRate;
    [Space(10)]
    [Header("Human Mid-Air Move")]
    public float airTargetSpeed;
    public float airAccelerationRate;
    public float airDecelerationRate;
    public float airTurnAccelerationRate;
}