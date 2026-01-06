using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;


public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// Based on the following tutorial: https://www.youtube.com/watch?v=wmTCWMcjIzo
    /// </summary>
    
    
    private CinemachineCamera _activeCamera;
    
    [SerializeField]
    private List<CinemachineCamera> cameras;

    public void RegisterCamera(CinemachineCamera camera)
    {
        cameras.Add(camera);
    }

    public void UnregisterCamera(CinemachineCamera camera)
    {
        cameras.Remove(camera);
    }

    public void ChangeCamera(CinemachineCamera camera, Transform trackingTarget)
    {
        _activeCamera = camera;
        foreach (var c in cameras)
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
