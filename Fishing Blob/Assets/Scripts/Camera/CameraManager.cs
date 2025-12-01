using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;


public class CameraManager : MonoBehaviour
{
    
    private CinemachineCamera _activeCamera = null;
    
    [SerializeField]
    private List<CinemachineCamera> _cameras;

    public void RegisterCamera(CinemachineCamera camera)
    {
        _cameras.Add(camera);
    }

    public void UnregisterCamera(CinemachineCamera camera)
    {
        _cameras.Remove(camera);
    }

    public void ChangeCamera(CinemachineCamera camera, Transform trackingTarget)
    {
        _activeCamera = camera;
        foreach (var c in _cameras)
        {
            if (c != camera)
            {
                c.Priority = 0;
            }
        }
        _activeCamera.Priority = 100;
        _activeCamera.Follow = trackingTarget;
    }
}
