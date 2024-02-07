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
    private Camera mainCamera = Camera.main; // TODO: Camera.main is a bad practice, use reference
    private void Start()
    {
        playerTransform = transform.parent;
    }

    private void FixedUpdate()
    {
        if (currentControlScheme == "Keyboard + Mouse")
        {
            // TODO: Camera.main is a bad practice, use reference
            mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            Vector3 position = playerTransform.position;
            
            Vector3 positionDifference = (Vector3)mousePosition - position;
            Vector3 clampedPos = Vector3.ClampMagnitude(positionDifference, maxDistanceFromPlayer);
            transform.position = clampedPos + position;
        }
        else if (currentControlScheme == "Gamepad")
        {
            gamepadRightStickPosition = Gamepad.current.rightStick.ReadValue();
            Vector3 position = gamepadRightStickPosition * maxDistanceFromPlayer;
            transform.position = position + playerTransform.position;
        }
    }

    // TODO: Unused Method???
    private void OnControlsChanged(PlayerInput playerInput)
    {
        currentControlScheme = playerInput.currentControlScheme;
    }
}
