using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowablePickUp : MonoBehaviour
{
    public GameObject throwablePrefab;
    public Sprite inHandSprite;

    public GameObject GetInHandPrefab()
    {
        return throwablePrefab;
    }

    public Sprite GetInHandSprite()
    {
        return inHandSprite;
    }
}
