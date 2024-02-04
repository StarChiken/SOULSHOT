using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnimationObject : MonoBehaviour
{
    private float lifeTime;
    private float totalLifeTime;

    private void Start()
    {
        lifeTime = GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
    }

    private void FixedUpdate()
    {
        totalLifeTime += Time.fixedDeltaTime;

        if (totalLifeTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
