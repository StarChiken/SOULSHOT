using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player
{
    private static Player instance = null;

    private Form playerForm;

    private GameObject playerObject;
    private Transform playerFireTracker;

    public static event Action<Form> onFormChange;
    public static event Action<bool> onGroundedChange;
    public static event Action<bool> onHoldChange;
    public static event Action<bool, GameObject, Sprite> onThrowableHoldChange;
    public static event Action<FireType, Vector2> onFireTriggered;
    public static event Action<GameObject> onPlayerObjectChange;
    public static event Action<Transform> onPlayerFireTrackerChange;

    private bool isGrounded = true;
    private bool isCrouching = false;
    private bool isHolding = true;
    private bool hasBullet = true;
    private bool hasGun = true;

    private Player()
    {
        playerForm = Form.Human;
    }

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }
    }

    public Form GetPlayerForm()
    {
        return playerForm;
    }

    public void SetPlayerForm(Form _form)
    {
        playerForm = _form;
        onFormChange?.Invoke(_form);
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void SetIsGrounded(bool _isGrounded)
    {
        isGrounded = _isGrounded;
        onGroundedChange?.Invoke(_isGrounded);
    }

    public bool GetIsCrouching()
    {
        return isCrouching;
    }

    public void SetIsCrouching(bool _isCrouching)
    {
        isCrouching = _isCrouching;
    }

    public bool GetHasBullet()
    {
        return hasBullet;
    }

    public void SetHasBullet(bool _hasBullet)
    {
        hasBullet = _hasBullet;
    }

    public bool GetHasGun()
    {
        return hasGun;
    }

    public void SetHasGun(bool _hasGun)
    {
        hasGun = _hasGun;
    }

    public void SetIsHoldingThrowable(bool _isHolding, GameObject holdPrefab, Sprite holdSprite)
    {
        isHolding = _isHolding;
        onThrowableHoldChange?.Invoke(_isHolding, holdPrefab, holdSprite);
    }

    public void SetIsHolding(bool _isHolding)
    {
        isHolding = _isHolding;
        onHoldChange?.Invoke(_isHolding);
    }

    public GameObject GetPlayerObject()
    {
        return playerObject;
    }

    public void SetPlayerObject(GameObject _playerObject)
    {
        playerObject = _playerObject;
        onPlayerObjectChange?.Invoke(playerObject);
    }

    public Transform GetPlayerFireTracker()
    {
        return playerFireTracker;
    }

    public void SetPlayerFireTracker(Transform _playerFireTracker)
    {
        playerFireTracker = _playerFireTracker;
        onPlayerFireTrackerChange?.Invoke(playerFireTracker);
    }

    public bool GetIsHolding()
    {
        return isHolding;
    }

    public void KillPlayer()
    {
        ResetEvents();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetEvents()
    {
        onFormChange = null;
        onGroundedChange = null;
        onHoldChange = null;
        onThrowableHoldChange = null;
        onFireTriggered = null;
    }

    public void SendOnFireTrigger(FireType _fireType, Vector2 _rotationVector)
    {
        onFireTriggered?.Invoke(_fireType, _rotationVector);
    }
}

public enum Form
{
    Human,
    Demon
}

public enum FireType
{
    Gun,
    ThrowGun,
    Throwable,
    Punch
}
