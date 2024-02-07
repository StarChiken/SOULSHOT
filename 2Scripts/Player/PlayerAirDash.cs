using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirDash : MonoBehaviour
{
    public float dashForce;

    public ParticleSystem dashParticles;

    private bool canDash = true;
    
    // TODO: Mistake? Use Player isGrounded
    private bool isGrounded = true;

    private Vector2 currentMoveVector;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Player.onGroundedChange += OnGroundedChange;
    }
    
    // TODO: Use Inheritance? also on PlayerDash
    private void OnDash()
    {
        if (canDash && !isGrounded && currentMoveVector.magnitude > 0 && Player.Instance.GetPlayerForm() == Form.Demon)
        {
            dashParticles.Play();

            Vector2 rotation = new Vector2(-currentMoveVector.x, -currentMoveVector.y);

            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            dashParticles.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            rb.velocity = Vector2.zero;
            rb.AddForce(dashForce * currentMoveVector.normalized, ForceMode2D.Impulse);
            canDash = false;
        }
    }

    private void OnMovement(InputValue inputValue)
    {
        currentMoveVector = inputValue.Get<Vector2>();
        if (currentMoveVector.y > 0)
        {
            currentMoveVector.y = 0;
        }
    }

    private void OnGroundedChange(bool _isGrounded)
    {
        isGrounded = _isGrounded;

        if (_isGrounded)
        {
            canDash = true;
        }
    }
}
