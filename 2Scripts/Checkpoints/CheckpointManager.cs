using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Can be made into non-MonoBehaviour
public class CheckpointManager : MonoBehaviour
{
    public Transform player;

    private SaveManager saveManager;

    private Dictionary<int, Checkpoint> checkpointDict = new Dictionary<int, Checkpoint>();

    private void Start()
    {
        saveManager = FindFirstObjectByType<SaveManager>();

        Checkpoint[] checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);

        foreach (Checkpoint checkpoint in checkpoints)
        {
            bool addedToDict = checkpointDict.TryAdd(checkpoint.GetCheckpointIndex(), checkpoint);

            if (!addedToDict)
            {
                Debug.LogError("Duplicate CheckpointIndex");
            }
            else
            {
                Debug.Log("Checkpoint " + checkpoint.GetCheckpointIndex() + " Added To Dictionary");
            }
        }

        checkpointDict[saveManager.GetCurrentCheckpointIndex()].SetAsStartCheckpoint();

        Vector2 startingCheckpointPos = checkpointDict[saveManager.GetCurrentCheckpointIndex()].GetCheckpointPosition();
        player.transform.position = new Vector3(startingCheckpointPos.x, startingCheckpointPos.y, 0);
    }

    public void SetCurrentCheckpointPosition(int checkpointIndex)
    {
        if (checkpointDict.ContainsKey(checkpointIndex))
        {
            saveManager.SetCurrentCheckpointIndex(checkpointIndex);
            saveManager.SaveGame();
        }
        else
        {
            Debug.LogError("Invalid Checkpoint Index");
        }
    }
}
