using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public ShatteredProjectile[] shatteredPieces;
    public float shatteredPiecesSpawnOffset;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player Bullet")
        {
            Destroy(col.gameObject);
            DestroyTarget();
        }
        else if (col.gameObject.tag == "Player Throwable")
        {
            DestroyTarget();
        }
    }

    private void DestroyTarget()
    {
        for (int i = 0; i < shatteredPieces.Length; i++)
        {    
            // TODO: Instantiate is a bad practice, use Factory design pattern and Pooling
            Projectile projectileInstanceScript = Instantiate(shatteredPieces[i].shatteredPiece, transform.position, Quaternion.identity).GetComponent<Projectile>();

            Vector2 velVector = new Vector2(Mathf.Cos(shatteredPieces[i].shatteredLaunchRotation * Mathf.Deg2Rad), Mathf.Sin(shatteredPieces[i].shatteredLaunchRotation * Mathf.Deg2Rad));
            
            projectileInstanceScript.FireProjectile(velVector, velVector * shatteredPiecesSpawnOffset);
        }

        Destroy(gameObject);
    }
}

[Serializable]
public class ShatteredProjectile
{
    public GameObject shatteredPiece;
    public float shatteredLaunchRotation;
}
