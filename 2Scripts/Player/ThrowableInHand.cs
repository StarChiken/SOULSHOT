using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableInHand : MonoBehaviour
{
    public GameObject throwableObjectPrefab;
    public Sprite emptySprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player.onThrowableHoldChange += OnThrowableHoldChange;
        Player.onFireTriggered += OnFireTriggered;
    }

    // TODO: Can be made private. Later use???
    public void ThrowObjectInHand(Vector2 rotationVector)
    {
        spriteRenderer.sprite = emptySprite;
        GameObject throwObjectInstance = Instantiate(throwableObjectPrefab, transform.position, Quaternion.identity);
        throwObjectInstance.GetComponent<Projectile>().FireProjectile(rotationVector.normalized, Vector3.zero);

        Player.Instance.SetPlayerForm(Form.Demon);
        Player.Instance.SetIsHolding(false);
    }

    // TODO: Can be made private. Later use???
    public void OnThrowableHoldChange(bool _isHolding,GameObject _throwableObjectPrefab, Sprite _inHandSprite)
    {
        if (_isHolding)
        {
            AddObjectToHand(_throwableObjectPrefab, _inHandSprite);
        }
        else
        {
            RemoveObjectFromHand();
        }
    }

    private void AddObjectToHand(GameObject _throwableObjectPrefab, Sprite _inHandSprite)
    {
        throwableObjectPrefab = _throwableObjectPrefab;
        spriteRenderer.sprite = _inHandSprite;
    }

    // TODO: Can be made private. Later use???
    public void RemoveObjectFromHand()
    {
        spriteRenderer.sprite = emptySprite;
    }

    private void OnFireTriggered(FireType _fireType, Vector2 _rotationVector)
    {
        if (_fireType == FireType.Throwable)
        {
            ThrowObjectInHand(_rotationVector);
        }
    }
}
