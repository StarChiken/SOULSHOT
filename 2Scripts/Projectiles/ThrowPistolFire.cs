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

    // TODO: Instantiate is a bad practice, use Factory design pattern and Pooling 
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (canFire)
        {
            canFire = false;
            
            Vector3 position = firePos.position;
            Vector3 pivotPosition = pivot.position;
            
            Projectile projectileInstanceScript = Instantiate(bulletPrefab, position, Quaternion.identity).GetComponent<Projectile>();
            projectileInstanceScript.FireProjectile(new Vector2(position.x - pivotPosition.x, position.y - pivotPosition.y).normalized, Vector3.zero);
            
            // Projectile projectileInstanceScript = Instantiate(bulletPrefab, firePos.position, Quaternion.identity).GetComponent<Projectile>();
            // projectileInstanceScript.FireProjectile(new Vector2(firePos.position.x - pivot.position.x, firePos.position.y - pivot.position.y).normalized, Vector3.zero);
        }
    }

    public void SetCanFire(bool _canFire)
    {
        canFire = _canFire;
        throwableScript.SetShouldBounce(_canFire);
    }
}
