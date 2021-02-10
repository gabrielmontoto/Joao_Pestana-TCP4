using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get;  set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
   [SerializeField] private float shakeTImer;
    [SerializeField] float intensidade;
    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float timer)
    {
        intensidade = intensity;
        CinemachineBasicMultiChannelPerlin CMBMCP = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CMBMCP.m_AmplitudeGain = intensity;
        shakeTImer = timer;
    }
    private void Update()
    {
        if(shakeTImer>0)
        shakeTImer -= Time.deltaTime;
        if(shakeTImer<=0)
        {
            CinemachineBasicMultiChannelPerlin CMBMCP = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            CMBMCP.m_AmplitudeGain = 0f;
          //  transform.rotation = Quaternion.identity;
         //   GetComponentInParent<Transform>().rotation = Quaternion.identity;
        }


    }
}