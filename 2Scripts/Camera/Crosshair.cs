using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    public float distanceFromPlayer;

    private Vector2 mousePosition = Vector2.zero;
    private Vector2 gamepadRightStickPosition;

    private Transform playerTransform;

    private string currentControlScheme = "";

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerTransform = transform.parent;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (currentControlScheme == "Gamepad")
        {
            gamepadRightStickPosition = Gamepad.current.rightStick.ReadValue();
            if (gamepadRightStickPosition.magnitude > 0.4f)
            {
                Vector3 position = gamepadRightStickPosition.normalized * distanceFromPlayer;
                transform.position = position + playerTransform.position;
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void OnControlsChanged(PlayerInput playerInput)
    {
        currentControlScheme = playerInput.currentControlScheme;
    }
}
