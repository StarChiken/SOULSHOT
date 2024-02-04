using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    public float flipOffset;
    public float aimSmoothTime;

    private float rotationVel;
    private float rotZ = 0;

    private string currentControlScheme = "";

    private Vector2 mousePosition;
    private Vector2 gamepadRightStickPosition;

    private Transform fireParticleEmitter;
    private Transform aimCenter;

    void Start()
    {
        fireParticleEmitter = GetComponentInChildren<ParticleSystem>().transform;
        aimCenter = transform.parent;
    }

    void Update()
    {
        if (currentControlScheme == "Keyboard + Mouse")
        {
            HandleMouseAim();
        }
        else if (currentControlScheme == "Gamepad")
        {
            HandleGamepadAim();
        }

        transform.eulerAngles = new Vector3(0, 0, Mathf.SmoothDampAngle(transform.eulerAngles.z, rotZ, ref rotationVel, aimSmoothTime));
    }

    private void HandleMouseAim()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (mousePosition.x < transform.parent.position.x - flipOffset || mousePosition.x > transform.parent.position.x + flipOffset)
        {
            Vector2 rotation = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            if (mousePosition.x < transform.parent.position.x)
            {
                rotZ += 180;
            }
        }

        if (mousePosition.x < transform.parent.position.x - flipOffset)
        {
            SetAimObjectsLocalScale(-1);
        }
        else if (mousePosition.x > transform.parent.position.x + flipOffset)
        {
            SetAimObjectsLocalScale(1);
        }
    }

    private void HandleGamepadAim()
    {
        gamepadRightStickPosition = Gamepad.current.rightStick.ReadValue();

        rotZ = Mathf.Atan2(gamepadRightStickPosition.y, gamepadRightStickPosition.x) * Mathf.Rad2Deg;

        if (gamepadRightStickPosition.x < 0)
        {
            rotZ += 180;
        }

        SetAimObjectsLocalScale(Mathf.Sign(gamepadRightStickPosition.x));
    }

    private void SetAimObjectsLocalScale(float relativeInputOffsetSign)
    {
        aimCenter.localScale = new Vector2(relativeInputOffsetSign, aimCenter.localScale.y);
        fireParticleEmitter.localScale = new Vector2(relativeInputOffsetSign, fireParticleEmitter.localScale.y);
    }

    public void OnFire()
    {
        if (Player.Instance.GetIsHolding())
        {
            if (Player.Instance.GetHasGun())
            {
                SendOnFireTrigger(FireType.Gun);
            }
            else
            {
                SendOnFireTrigger(FireType.Throwable);
            }
        }
        else
        {
            Player.Instance.SendOnFireTrigger(FireType.Punch, Vector2.zero);
        }
    }

    private void OnAltFire()
    {
        if (Player.Instance.GetIsHolding())
        {
            if (Player.Instance.GetHasGun())
            {
                SendOnFireTrigger(FireType.ThrowGun);
            }
            else
            {
                SendOnFireTrigger(FireType.Throwable);
            }
        }
    }

    private void SendOnFireTrigger(FireType fireType)
    {
        if (currentControlScheme == "Keyboard + Mouse")
        {
            Player.Instance.SendOnFireTrigger(fireType, new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y));
        }
        else if (currentControlScheme == "Gamepad")
        {
            Player.Instance.SendOnFireTrigger(fireType, gamepadRightStickPosition);
        }
    }

    private void OnControlsChanged(PlayerInput playerInput)
    {
        currentControlScheme = playerInput.currentControlScheme;
    }
}
