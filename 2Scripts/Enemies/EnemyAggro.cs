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

    private bool CheckPlayerDirection()
    {
        if (Mathf.Sign(playerFireTracker.transform.position.x - transform.position.x) == lookDirection)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckPlayerDistance()
    {
        if (Vector2.Distance(playerFireTracker.transform.position, transform.position) <= aggroDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckPlayerLOS()
    {
        Vector2 rotation = new Vector2(playerFireTracker.transform.position.x - transform.position.x, playerFireTracker.transform.position.y - transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rotation.normalized, Mathf.Infinity, rayCastMask);

        if (hit.transform != null && hit.transform.GetComponent<PlayerHorizontalMovement>() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    public void SetIsAggro(bool _isAggro)
    {
        isAggro = _isAggro;
        onAggroChange?.Invoke(isAggro);
    }
}
