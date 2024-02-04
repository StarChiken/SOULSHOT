using System;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [Header("Lifetime")]
    public float lifetimeBeforeStick;

    [Header("Wall Checks")]
    public float rayLookAheadDistance;
    public float minWallStickVelocity;

    public LayerMask layerMask;

    [Header("Object Assignment")]
    public GameObject groundObject;
    public GameObject inWallPrefab;

    [HideInInspector]
    public float currentZRotation;

    private bool hasBounced = false;
    private bool canStickInWall = false;
    private bool shouldBounce = false;
    private bool isFacingLeft = false;
    private bool hasCheckedDirection = false;

    private float lifetimeBeforeStickTimer = 0;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCol;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCol = GetComponent<CircleCollider2D>();

        groundObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!hasCheckedDirection)
        {
            hasCheckedDirection = true;
            if (rb.velocityX < 0)
            {
                isFacingLeft = true;
                spriteRenderer.flipX = isFacingLeft;
            }
        }

        CanStickInWallTimer();
    }

    private void CanStickInWallTimer()
    {
        if (lifetimeBeforeStickTimer >= lifetimeBeforeStick)
        {
            canStickInWall = true;
        }
        else
        {
            lifetimeBeforeStickTimer += Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            shouldBounce = true;
        }

        if (shouldBounce)
        {
            print("bounced");
            shouldBounce = false;
            hasBounced = true;
        }
        else
        {
            if (hasBounced)
            {
                SwapToGroundObject();
            }
            else if (canStickInWall)
            {
                if (col.gameObject.tag == "Ground")
                {
                    StickObjectInWall(col.GetContact(0).point);
                }
            }
            else
            {
                hasBounced = true;
            }
        }
    }

    private void StickObjectInWall(Vector2 colContactPoint)
    {
        int spriteIndex = Convert.ToInt32(spriteRenderer.sprite.name[spriteRenderer.sprite.name.Length - 1].ToString());

        GameObject instance = Instantiate(inWallPrefab, colContactPoint, Quaternion.identity);
        instance.GetComponent<PistolInWall>().SetSprite(spriteIndex, isFacingLeft);

        Destroy(gameObject);
    }

    private void SwapToGroundObject()
    {
        spriteRenderer.enabled = false;
        circleCol.enabled = false;

        groundObject.SetActive(true);
        groundObject.transform.eulerAngles = Vector3.forward * currentZRotation;
    }

    public void SetShouldBounce(bool _shouldBounce)
    {
        shouldBounce = _shouldBounce;
    }
}
