using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolInWall : MonoBehaviour
{
    public float totalLifetime;
    public float spawnPosOffset;

    [Header("Camera Shake")]
    public float camShakeTime;
    public float camShakeAmplitude;

    public Sprite[] sprites;

    private float currentLifetime;

    private SpriteRenderer spriteRenderer;

    private ParticleSystem inWallParticles;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inWallParticles = GetComponentInChildren<ParticleSystem>();

        FindFirstObjectByType<CameraShake>().ShakeCamera(camShakeTime, camShakeAmplitude);
    }

    private void FixedUpdate()
    {
        currentLifetime += Time.fixedDeltaTime;

        if (currentLifetime >= totalLifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetSprite(int spriteIndex, bool isFacingLeft)
    {
        spriteRenderer.sprite = sprites[spriteIndex];

        if (isFacingLeft)
        {
            spriteRenderer.flipX = true;
        }

        inWallParticles.Play();
    }
}
