using UnityEngine;

public class FollowMainCamera : MonoBehaviour
{
    [SerializeField]
    private Camera mainCameraToFollow;
    private Camera thisCamera;

    void Awake()
    {
        thisCamera = GetComponent<Camera>();
    }
    void LateUpdate()
    {
        Debug.Log(thisCamera);
        if (mainCameraToFollow == null) return;

        transform.position = mainCameraToFollow.transform.position;
        transform.rotation = mainCameraToFollow.transform.rotation;
        thisCamera.orthographicSize = mainCameraToFollow.orthographicSize;
    }
}
