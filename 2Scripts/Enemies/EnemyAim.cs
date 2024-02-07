using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    [Header("Firing")]
    public float totalShotTime;
    public float lockInRotationTime;
    public Vector2 fireOffset;

    [Header("Aiming")]
    public float aimSmoothTime;
    public float idleRotation;
    public float minTurretRot;
    public float maxTurretRot;
    public float aimDistance;

    [Header("Assignment")]
    public GameObject bulletPrefab;
    public EnemyAggro aggroScript;

    private Transform playerFireTracker;

    private bool isAiming = false;
    private bool isAggro = false;

    private float shotTimer = 0;
    private float rotationVel = 0;

    public event Action<bool> onAiming;
    public event Action onFire;

    private void Start()
    {
        aggroScript.onAggroChange += OnAggroChange;
        Player.onPlayerFireTrackerChange += OnFireTrackerChange;
    }


    private void FixedUpdate()
    {
        if (isAggro && CheckPlayerDistance())
        {
            shotTimer += Time.fixedDeltaTime;

            if (shotTimer >= totalShotTime)
            {
                FireBullet(); // TODO: Check in profilers. The Unholy Trinity, GetComponent + RayCast + Update.
            }
            else if (shotTimer < lockInRotationTime)
            {
                RotateGun();
            }
        }
        else if (shotTimer != 0)
        {
            isAiming = false;

            onAiming?.Invoke(false);

            shotTimer = 0;
        }
        else
        {
            transform.eulerAngles = GetSmoothedRotation(idleRotation);
        }
    }

    private void FireBullet()
    {
        isAiming = false;
        
        onAiming?.Invoke(false);
        onFire?.Invoke();
        
        // TODO: Instantiate is a bad practice, use Factory design pattern and Pooling
        // GetComponent in Raycast is a bad practice, always use a script name like this, else would be very slow 

        Projectile projectileInstance = Instantiate(bulletPrefab, transform.position + new Vector3(aggroScript.GetLookDirection()
            * fireOffset.x, fireOffset.y, 0), transform.rotation).GetComponent<Projectile>();
        
        Vector2 fireDirection = Vector2.zero;
        Vector3 transformEulerAngles = transform.eulerAngles;
        
        if (aggroScript.GetLookDirection() == 1)
        {
            
            fireDirection = new Vector2(Mathf.Cos((transformEulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((transformEulerAngles.z) * Mathf.Deg2Rad)).normalized;
        }
        else if (aggroScript.GetLookDirection() == -1)
        {
            
            fireDirection = new Vector2(Mathf.Cos((transformEulerAngles.z + 180) * Mathf.Deg2Rad), Mathf.Sin((transformEulerAngles.z + 180) * Mathf.Deg2Rad)).normalized;
        }
        
        projectileInstance.FireProjectile(fireDirection, Vector2.zero);
        
        shotTimer = 0;
    }

    private void RotateGun()
    {
        if (!isAiming)
        {
            isAiming = true;
            onAiming?.Invoke(true);
        }

        Vector3 transformPosition = transform.position;
        Vector3 fireTrackerTransformPosition = playerFireTracker.transform.position;
        
        Vector2 rotation = new Vector2(transformPosition.x - fireTrackerTransformPosition.x, transformPosition.y - fireTrackerTransformPosition.y);

        float targetZRot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
        
        // TODO: Possible floating point error
        if (Mathf.Sign(rotation.x) == -1 && Mathf.Sign(rotation.y) == -1)
        {
            targetZRot = -targetZRot;
        }

        if (aggroScript.GetLookDirection() == 1)
        {
            targetZRot -= 180;
        }

        // TODO: Possible floating point error
        if (Mathf.Sign(rotation.x) == -1 && Mathf.Sign(rotation.y) == -1)
        {
            targetZRot = -targetZRot;
        }

        targetZRot = Mathf.Clamp(targetZRot, minTurretRot, maxTurretRot);

        // TODO: Possible floating point error
        if (targetZRot == minTurretRot)
        {
            transform.eulerAngles = GetSmoothedRotation(minTurretRot);
        }
        else if (targetZRot == maxTurretRot)
        {
            transform.eulerAngles = GetSmoothedRotation(maxTurretRot);
        }
        else
        {
            transform.eulerAngles = GetSmoothedRotation(targetZRot);
        }
    }

    private Vector3 GetSmoothedRotation(float targetRotation)
    {
        return new Vector3(0, 0, Mathf.SmoothDampAngle(transform.eulerAngles.z, targetRotation, ref rotationVel, aimSmoothTime, Mathf.Infinity, Time.fixedDeltaTime));
    }

    private bool CheckPlayerDistance()
    {
        return Vector2.Distance(playerFireTracker.transform.position, transform.position) <= aimDistance;
        
        // if (Vector2.Distance(playerFireTracker.transform.position, transform.position) <= aimDistance)
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
    }

    // TODO: Unused Method???
    public bool GetIsAiming()
    {
        return isAiming;
    }

    private void OnFireTrackerChange(Transform _playerFireTracker)
    {
        playerFireTracker = _playerFireTracker;
    }

    private void OnAggroChange(bool _isAggro)
    {
        isAggro = _isAggro;
    }
}
