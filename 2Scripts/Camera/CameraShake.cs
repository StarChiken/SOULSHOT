using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private bool isShaking = false;

    private float shakeTime = 0;
    private float shakeTimer = 0;

    private CinemachineVirtualCamera virtualCam;

    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void FixedUpdate()
    {
        if (isShaking)
        {
            shakeTimer += Time.fixedDeltaTime;

            if (shakeTimer >= shakeTime)
            {
                isShaking = false;
                virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float _shakeTime, float _shakeAmplitude)
    {
        shakeTime = _shakeTime;
        shakeTimer = 0;
        isShaking = true;
        virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _shakeAmplitude;
    }
}
