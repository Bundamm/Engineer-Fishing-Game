using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraRegister : MonoBehaviour
{
    [SerializeField]
    private CameraManager camManager;
    private void OnEnable()
    {
        camManager.RegisterCamera(GetComponent<CinemachineCamera>());
        
    }

    private void OnDisable()
    {
        camManager.UnregisterCamera(GetComponent<CinemachineCamera>());
    }
}
