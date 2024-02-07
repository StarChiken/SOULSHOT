using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    [SerializeField] private float aggroDistance;

    [Range(-1, 1)]
    [SerializeField] private int lookDirection;

    public LayerMask rayCastMask;

    [SerializeField] bool canStayAggroOnDirectionChange;

    private bool isAggro = false;

    private Transform playerFireTracker;

    public event Action<bool> onAggroChange;

    void Start()
    {
        Player.onPlayerFireTrackerChange += OnFireTrackerChange;

        if (lookDirection == 0)
        {
            Debug.LogError("Invalid Look Direction");
        }
    }

    void FixedUpdate()
    {
        if (canStayAggroOnDirectionChange)
        {
            if (playerFireTracker != null && CheckPlayerDistance() && CheckPlayerLOS())
            {
                if (!isAggro && CheckPlayerDirection())
                {
                    isAggro = true;
                    onAggroChange?.Invoke(isAggro);
                }
            }
            else if (isAggro)
            {
                isAggro = false;
                onAggroChange?.Invoke(isAggro);
            }
        }
        else
        {
            if (playerFireTracker != null && CheckPlayerDirection() && CheckPlayerDistance() && CheckPlayerLOS())
            {
                if (!isAggro)
                {
                    isAggro = true;
                    onAggroChange?.Invoke(isAggro);
                }
            }
            else if(isAggro)
            {
                isAggro = false;
                onAggroChange?.Invoke(isAggro);
            }
        }
    }
    // TODO: Possible floating point error
    private bool CheckPlayerDirection()
    {
        return Mathf.Sign(playerFireTracker.transform.position.x - transform.position.x) == lookDirection;

        // if (Mathf.Sign(playerFireTracker.transform.position.x - transform.position.x) == lookDirection)
        // {
        //     return true;
        // } 
        //
        // return false;
    }

    private bool CheckPlayerDistance()
    {
        return Vector2.Distance(playerFireTracker.transform.position, transform.position) <= aggroDistance;

        // if (Vector2.Distance(playerFireTracker.transform.position, transform.position) <= aggroDistance)
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
    }

    private bool CheckPlayerLOS()
    {
        Vector3 fireTrackerTransformPosition = playerFireTracker.transform.position;
        Vector3 transformPosition = transform.position;
        Vector2 rotation = new Vector2(fireTrackerTransformPosition.x - transformPosition.x, fireTrackerTransformPosition.y - transformPosition.y);

        RaycastHit2D hit = Physics2D.Raycast(transformPosition, rotation.normalized, Mathf.Infinity, rayCastMask);

        // TODO: GetComponent in Raycast is a bad practice, very slow 
        return hit.transform != null && hit.transform.GetComponent<PlayerHorizontalMovement>() != null;
        
        // if (hit.transform != null && hit.transform.GetComponent<PlayerHorizontalMovement>() != null)
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
    }

    private void OnFireTrackerChange(Transform _playerFireTracker)
    {
        playerFireTracker = _playerFireTracker;
    }

    public float GetAggroDistance()
    {
        return aggroDistance;
    }

    public void SetLookDirection(int _lookDirection)
    {
        if (_lookDirection == 1)
        {
            lookDirection = 1;
        }
        else if (_lookDirection == -1)
        {
            lookDirection = -1;
        }
    }

    public int GetLookDirection()
    {
        return lookDirection;
    }

    // TODO: Unused Method???
    public void SetIsAggro(bool _isAggro)
    {
        isAggro = _isAggro;
        onAggroChange?.Invoke(isAggro);
    }
}
