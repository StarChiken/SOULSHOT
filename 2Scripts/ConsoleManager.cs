using System;
using UnityEngine;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    public TextMeshProUGUI consoleInputText;

    private bool inConsole = false;

    private string currentInput = "";

    private GameObject consoleCanvas;

    private void Start()
    {
        consoleCanvas = transform.GetChild(0).gameObject;
        consoleCanvas.SetActive(false);
    }

    private void Update()
    {
        if (inConsole)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                string commandType = "";
                string commandValue = "";

                ParseCommandString(ref commandType, ref commandValue);

                float newTimeScale = 1;

                if (commandType == "setCheckpoint")
                {
                    HandleSetCheckpointCommand(commandValue);
                }
                else if (commandType == "kill")
                {
                    HandleKillCommand(commandValue);
                }
                else if (commandType == "setTimeScale")
                {
                    newTimeScale = HandleSetTimeScaleCommand(commandValue);
                }
                else
                {
                    Debug.Log("Invalid Console Command");
                }
                
                SetInConsole(false, newTimeScale);
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) && currentInput.Length != 0)
            {
                currentInput = currentInput.Remove(currentInput.Length - 1);
                consoleInputText.text = currentInput;
            }
            else
            {
                currentInput += Input.inputString;
                consoleInputText.text = currentInput;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SetInConsole(true, 0);
        }
    }

    private float HandleSetTimeScaleCommand(string commandValue)
    {
        bool wasParsed = float.TryParse(commandValue, out var newTimeScale);

        if (wasParsed)
        {
            Debug.Log("Time Scale Set: " + newTimeScale);
        }
        else
        {
            LogInvalidResponse(commandValue);
        }

        return newTimeScale;
    }

    private void HandleKillCommand(string commandValue)
    {
        if (commandValue == "player")
        {
            FindAnyObjectByType<PlayerDeath>().KillPlayer();
        }
        else if (commandValue.Length == 0)
        {
            FindAnyObjectByType<PlayerDeath>().KillPlayer();
        }
        else
        {
            LogInvalidResponse(commandValue);
        }
    }

    private void HandleSetCheckpointCommand(string commandValue)
    {
        bool wasParsed = int.TryParse(commandValue, out int checkpointIndex);

        if (wasParsed)
        {
            FindAnyObjectByType<CheckpointManager>().SetCurrentCheckpointPosition(checkpointIndex);
            Debug.Log("Checkpoint Set: " + checkpointIndex);
        }
        else
        {
            LogInvalidResponse(commandValue);
        }
    }

    private void LogInvalidResponse(string commandValue)
    {
        Debug.Log("Invalid Console Command Input: " + commandValue);
    }

    private void ParseCommandString(ref string commandType, ref string commandValue)
    {
        for (int i = 0; i < currentInput.Length; i++)
        {
            if (currentInput[i] != ' ')
            {
                commandType += currentInput[i];
            }
            else
            {
                currentInput = currentInput.Remove(0, i + 1);
                commandValue = currentInput;
                break;
            }
        }
    }

    public void SetInConsole(bool _inConsole, float _timeScale)
    {
        consoleCanvas.SetActive(_inConsole);
        inConsole = _inConsole;

        currentInput = "";
        consoleInputText.text = currentInput;

        Time.timeScale = _timeScale;
    }
}
