using System;
using UnityEngine;

public class ThrowPistolFire : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform pivot;
    public Transform firePos;

    private bool canFire = false;

    private ThrowableObject throwableScript;

    private void Awake()
    {
        throwableScript = GetComponent<ThrowableObject>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (canFire)
        {
            canFire = false;
            Projectile projectileInstanceScript = Instantiate(bulletPrefab, firePos.position, Quaternion.identity).GetComponent<Projectile>();
            projectileInstanceScript.FireProjectile(new Vector2(firePos.position.x - pivot.position.x, firePos.position.y - pivot.position.y).normalized, Vector3.zero);
        }
    }

    public void SetCanFire(bool _canFire)
    {
        canFire = _canFire;
        throwableScript.SetShouldBounce(_canFire);
    }
}
