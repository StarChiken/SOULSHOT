using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile
{
    public string currentSceneName;
    public int currentCheckpointIndex;

    public SaveFile (string _currentSceneName, int _currentCheckpointIndex)
    {
        currentSceneName = _currentSceneName;
        currentCheckpointIndex = _currentCheckpointIndex;
    }

    public string ConvertToJSONString()
    {
        return JsonUtility.ToJson(this, true);
    }
}
