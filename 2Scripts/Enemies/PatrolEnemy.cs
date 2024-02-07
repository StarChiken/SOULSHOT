using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed;
    public float wallCheckRaycastOffset;
    public LayerMask wallCheckRaycastMask;

    [Header("Assignment")]
    public Transform fireParticleEmitter;
    public Transform aimCenter;

    private bool isAggro = false;
    private bool isAiming = false;

    private int moveDir = 1;

    private EnemyState currentState = EnemyState.Patrolling;

    private Transform playerTransform;

    private Rigidbody2D rb;

    private EnemyAggro aggroScript;

    private EnemyAim aimScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        aimScript = GetComponentInChildren<EnemyAim>();
        aggroScript = GetComponent<EnemyAggro>();

        aggroScript.onAggroChange += OnAggroChange;
        aimScript.onAiming += OnAimingChange;
        Player.onPlayerObjectChange += OnPlayerObjectChange;
    }

    void FixedUpdate()
    {
        if (currentState == EnemyState.Patrolling)
        {
            RaycastHit2D rightWallCheckHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(wallCheckRaycastOffset, 0), Vector2.right, 0.005f, wallCheckRaycastMask);

            if (rightWallCheckHit.transform != null)
            {
                moveDir = -1;
            }
            else
            {
                RaycastHit2D leftWallCheckHit = Physics2D.Raycast((Vector2)transform.position - new Vector2(wallCheckRaycastOffset, 0), Vector2.left, 0.005f, wallCheckRaycastMask);

                if (leftWallCheckHit.transform != null)
                {
                    moveDir = 1;
                }
            }
        }
        else if (currentState == EnemyState.Pursuing)
        {
            moveDir = (int)Mathf.Sign(playerTransform.position.x - transform.position.x);
            print(moveDir);
        }

        if (currentState == EnemyState.Patrolling)
        {
            rb.velocityX = moveSpeed * moveDir;
        }
        else if (currentState == EnemyState.Pursuing)
        {
            rb.velocityX = moveSpeed * moveDir;
        }

        aggroScript.SetLookDirection(moveDir);
        SetAimObjectsLocalScale(moveDir);
    }

    private void OnAggroChange(bool _isAggro)
    {
        isAggro = _isAggro;
        UpdateEnemyState();
    }

    private void OnAimingChange(bool _isAiming)
    {
        isAiming = _isAiming;
        UpdateEnemyState();
    }

    private void OnPlayerObjectChange(GameObject _playerObject)
    {
        playerTransform = _playerObject.transform;
    }

    // TODO: Inaccessible, rigid. Make its own script
    private enum EnemyState
    {
        Patrolling,
        Pursuing,
        Aiming,
        Dazed
    }

    private void SetAimObjectsLocalScale(int relativeInputOffsetSign)
    {
        Vector3 aimCenterLocalScale = aimCenter.localScale;
        aimCenterLocalScale = new Vector2(relativeInputOffsetSign * Mathf.Abs(aimCenterLocalScale.x), aimCenterLocalScale.y);
        
        aimCenter.localScale = aimCenterLocalScale;
        
        Vector3 particleEmmiterLocalScale = fireParticleEmitter.localScale;
        
        particleEmmiterLocalScale = new Vector2(relativeInputOffsetSign * Mathf.Abs(particleEmmiterLocalScale.x), particleEmmiterLocalScale.y);
        
        fireParticleEmitter.localScale = particleEmmiterLocalScale;
    }

    private void UpdateEnemyState()
    {
        if (isAggro)
        {
            if (isAiming)
            {
                currentState = EnemyState.Aiming;
                print("isAiming");
            }
            else
            {
                currentState = EnemyState.Pursuing;
                print("isPursuing");
            }
        }
        else
        {
            isAiming = false;
            currentState = EnemyState.Patrolling;
        }
    }
}
