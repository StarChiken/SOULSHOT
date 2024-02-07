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
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    private void FixedUpdate()
    {
        if (isShaking)
        {
            shakeTimer += Time.fixedDeltaTime;

            if (shakeTimer >= shakeTime)
            {
                isShaking = false;
                
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
              //  virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float _shakeTime, float _shakeAmplitude)
    {
        shakeTime = _shakeTime;
        shakeTimer = 0;
        isShaking = true;
        
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _shakeAmplitude;
      //  virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _shakeAmplitude;
    }
}
