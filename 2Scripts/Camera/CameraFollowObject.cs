using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowObject : MonoBehaviour
{
    public float maxDistanceFromPlayer;

    private Vector2 mousePosition = Vector2.zero;
    private Vector2 gamepadRightStickPosition;

    private Transform playerTransform;

    private string currentControlScheme = "";

    private void Start()
    {
        playerTransform = transform.parent;
    }

    private void FixedUpdate()
    {
        if (currentControlScheme == "Keyboard + Mouse")
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 positionDifference = (Vector3)mousePosition - playerTransform.position;
            Vector3 clampedPos = Vector3.ClampMagnitude(positionDifference, maxDistanceFromPlayer);
            transform.position = clampedPos + playerTransform.position;
        }
        else if (currentControlScheme == "Gamepad")
        {
            gamepadRightStickPosition = Gamepad.current.rightStick.ReadValue();
            Vector3 position = gamepadRightStickPosition * maxDistanceFromPlayer;
            transform.position = position + playerTransform.position;
        }
    }

    private void OnControlsChanged(PlayerInput playerInput)
    {
        currentControlScheme = playerInput.currentControlScheme;
    }
}
