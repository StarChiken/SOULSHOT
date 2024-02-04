using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileForce;
    public float lifetimeAfterHit;
    public float maxLifetime;

    private bool hasHit = false;

    private float afterHitLifetime = 0;
    private float currentLifetime = 0;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FireProjectile(Vector2 projectileVelVector, Vector3 relativeOffset)
    {
        transform.position += relativeOffset;
        rb.AddForce(projectileVelVector * projectileForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        hasHit = true;
    }

    private void FixedUpdate()
    {
        currentLifetime += Time.fixedDeltaTime;

        if (currentLifetime >= maxLifetime)
        {
            Destroy(gameObject);
        }
        else if (hasHit)
        {
            afterHitLifetime += Time.fixedDeltaTime;

            if (afterHitLifetime >= lifetimeAfterHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
