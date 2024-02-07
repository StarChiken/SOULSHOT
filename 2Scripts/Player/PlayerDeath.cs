using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeath : MonoBehaviour
{
    public float deathWaitTime;

    [Space(10)]
    [Header("Disable Objects")]
    public Sprite emptySprite;
    public SpriteRenderer armSpriteRenderer;

    private PlayerInput playerInput;

    private Animator playerBodyAnimator;

    private Rigidbody2D rb;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerBodyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SetIsHoldingFrameBuffer());
    }

    // TODO: Should be async? Not refactored yet or better optimized as Coroutine?
    IEnumerator SetIsHoldingFrameBuffer()
    {
        yield return new WaitForEndOfFrame();
        Player.Instance.SetIsHolding(true);
        Player.Instance.SetPlayerObject(gameObject);
        // Find During Coroutine is bad, inviting performance issues
        Player.Instance.SetPlayerFireTracker(GameObject.Find("Fire Tracker").transform); 
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy Bullet")
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        playerInput.enabled = false;

        RemoveObjectsFromHand();

        HideSprites();

        StopMovement();

        DeathTimer();
    }

    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    private void HideSprites()
    {
        armSpriteRenderer.sprite = emptySprite;
        playerBodyAnimator.SetTrigger("isDead");
    }

    private void RemoveObjectsFromHand()
    {
        Player.Instance.SetIsHolding(false);
        Player.Instance.SetIsHoldingThrowable(false, null, null);
    }

    private async void DeathTimer()
    {
        await Awaitable.WaitForSecondsAsync(deathWaitTime);
        Player.Instance.KillPlayer();
    }

    private void OnApplicationQuit()
    {
        Player.Instance.ResetEvents();
    }
}
