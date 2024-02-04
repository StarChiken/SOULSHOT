using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        Player.onFireTriggered += OnFireTriggered;
    }

    public void Punch()
    {
        animator.SetTrigger("punchTrigger");
    }

    private void OnFireTriggered(FireType _fireType, Vector2 _rotationVector)
    {
        if (_fireType == FireType.Punch)
        {
            Punch();
        }
    }
}
