using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;

    private bool hasBeenActivated = false;

    private CheckpointManager checkpointManager;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        checkpointManager = FindFirstObjectByType<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!hasBeenActivated && col.transform.tag == "Player")
        {
            checkpointManager.SetCurrentCheckpointPosition(checkpointIndex);
            hasBeenActivated = true;
            animator.SetTrigger("turnOnCheckpoint");

            print("Set Current Checkpoint To: " + checkpointIndex);
        }
    }

    public void SetAsStartCheckpoint()
    {
        animator.SetTrigger("startCheckpoint");
        hasBeenActivated = true;
    }

    public int GetCheckpointIndex()
    {
        return checkpointIndex;
    }

    public Vector2 GetCheckpointPosition()
    {
        return transform.position;
    }

    public bool GetHasBeenActivated()
    {
        return hasBeenActivated;
    }
}
