using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private SaveFile saveFile;

    private string saveFilePath;
    private void Awake()
    {
        saveFilePath = $"{Application.persistentDataPath}/saveFile.save";
        if (File.Exists(saveFilePath))
        {
            string saveJSONText = File.ReadAllText(saveFilePath);
            saveFile = JsonUtility.FromJson<SaveFile>(saveJSONText);
        }
        else
        {
            saveFile = new SaveFile(SceneManager.GetActiveScene().name, 0);
            SaveGame();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Current Level Name: " + GetCurrentLevelName() + "\nCurrent Checkpoint Index: " + GetCurrentCheckpointIndex());
        }
    }

    public void SaveGame()
    {
        saveFile.currentSceneName = SceneManager.GetActiveScene().name;
        string dataText = saveFile.ConvertToJSONString();
        File.WriteAllText(saveFilePath, dataText);
    }

    public string GetCurrentLevelName()
    {
        return saveFile.currentSceneName;
    }

    public int GetCurrentCheckpointIndex()
    {
        return saveFile.currentCheckpointIndex;
    }

    public void SetCurrentCheckpointIndex(int _currentCheckpointIndex)
    {
        saveFile.currentCheckpointIndex = _currentCheckpointIndex;
    }
}
