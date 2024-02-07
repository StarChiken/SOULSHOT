using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [Header("Average Framerate Log")]
    public bool enableAverageFramerateLogs = false;
    public float averageFramerateLogTime = 0;

    [Space(10)]
    [Header("Trail Renderer")]
    public bool enablePlayerTrailRenderer = false;
    public Vector3 spawnOffset = Vector3.zero;
    public GameObject trailRendererPrefab;
    public Transform playerTransform;

    [Space(10)]
    [Header("Time Augmentation")]
    public bool changeTimeScale = false;
    public float timeScale = 1;

    [Space(10)]
    [Header("Pick-Up Spawning")]
    public bool enablePickUpSpawning;
    public KeyCode pistolSpawnKey;
    public KeyCode randomThrowableSpawnKey;
    public GameObject pistolPickUpPrefab;
    public GameObject[] throwables;

    [Space(10)]
    [Header("Enemy Spawning")]
    public bool enableEnemySpawning;
    public KeyCode targetSpawnKey;
    public GameObject targetEnemyPrefab;

    private bool hasSpawnedTrailRenderer = false;

    private int frameQuantity = 0;
    private int frameCount = 0;

    private float currentAvgFPS = 0;
    private float time = 0;

    private float decelTrackTimer;

    private GameObject trailRendererInstance;

    private Camera mainCamera = Camera.main; // TODO: Camera.main is a bad practice, use reference
    private void Start()
    {
        ChangeTimeScale();
    }

    private void ChangeTimeScale()
    {
        if (changeTimeScale)
        {
            Time.timeScale = timeScale;
        }
    }

    void Update()
    {
        PrintAverageFramerate();
        PickUpSpawning();
        EnemySpawning();
    }

    private void PickUpSpawning()
    {
        if (enablePickUpSpawning)
        {
            if (Input.GetKeyDown(pistolSpawnKey))
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Instantiate(pistolPickUpPrefab, mousePos, Quaternion.identity);
            }
            else if (Input.GetKeyDown(randomThrowableSpawnKey))
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Instantiate(throwables[Random.Range(0, throwables.Length)], mousePos, Quaternion.identity);
            }
        }
    }

    private void EnemySpawning()
    {
        if (enablePickUpSpawning)
        {
            if (Input.GetKeyDown(targetSpawnKey))
            {
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Instantiate(targetEnemyPrefab, mousePos, Quaternion.identity);
            }
        }
    }

    private void FixedUpdate()
    {
        HandlePlayerTrailRendererSpawning();
    }

    private void HandlePlayerTrailRendererSpawning()
    {
        if (enablePlayerTrailRenderer && !hasSpawnedTrailRenderer)
        {
            trailRendererInstance = Instantiate(trailRendererPrefab, playerTransform.position + spawnOffset, Quaternion.identity, playerTransform);
            hasSpawnedTrailRenderer = true;
        }
        else if (!enablePlayerTrailRenderer && hasSpawnedTrailRenderer)
        {
            Destroy(trailRendererInstance);
            hasSpawnedTrailRenderer = false;
        }
    }

    private void PrintAverageFramerate()
    {
        if (enableAverageFramerateLogs)
        {
            ++frameCount;
            time += Time.deltaTime;
            if (time >= averageFramerateLogTime)
            {
                Debug.Log(UpdateCumulativeMovingAverageFPS(frameCount / time));
                frameCount = 0;
                time -= averageFramerateLogTime;
            }
        }
    }

    float UpdateCumulativeMovingAverageFPS(float newFPS)
    {
        frameQuantity++;
        currentAvgFPS += (newFPS - currentAvgFPS) / frameQuantity;

        return currentAvgFPS;
    }
}
