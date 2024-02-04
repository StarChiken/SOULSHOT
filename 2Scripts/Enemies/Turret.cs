using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Prefab Assignment")]
    public GameObject pistolPickUpPrefab;
    public GameObject explosionPrefab;

    private LineRenderer laserLine;
    private Animator animator;

    private EnemyAim aimScript;
    private EnemyAggro aggroScript;

    private void Start()
    {
        aimScript = GetComponent<EnemyAim>();
        aggroScript = GetComponent<EnemyAggro>();

        laserLine = GetComponentInChildren<LineRenderer>();
        animator = GetComponentInChildren<Animator>();

        laserLine.enabled = false;
        laserLine.SetPosition(1, aggroScript.GetAggroDistance() * 0.5f * Vector3.left);

        aggroScript.onAggroChange += OnAggroChanged;
        aimScript.onFire += OnFire;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player Bullet")
        {
            Destroy(col.gameObject);
            DestroyTurret();
        }
        else if (col.gameObject.tag == "Player Throwable")
        {
            DestroyTurret();
        }
    }

    private void DestroyTurret()
    {
        transform.parent.GetComponent<SpriteRenderer>().color = Color.gray;
        Instantiate(pistolPickUpPrefab, transform.position, Quaternion.identity);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnAggroChanged(bool isAggro)
    {
        if (isAggro)
        {
            laserLine.enabled = true;
        }
        else
        {
            laserLine.enabled = false;
        }
    }

    private void OnFire()
    {
        animator.SetTrigger("fireGun");
    }
}
