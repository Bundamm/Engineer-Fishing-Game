using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    /// <summary>
    ///  Based on the following tutorial: https://www.youtube.com/watch?v=zit45k6CUMk
    /// </summary>
    
    private float _startCameraPosX;
    private float _startLayerX;
    [SerializeField] 
    private Transform targetCamera;
    [SerializeField] 
    [Range(0f, 1f)]
    private float parallaxAmount;
    
    void Start()
    {
        _startCameraPosX = targetCamera.position.x;
        _startLayerX = transform.position.x;
    }
    
    void LateUpdate()
    {
        float distanceMoved = targetCamera.position.x - _startCameraPosX;
        float newX = _startLayerX + (distanceMoved * parallaxAmount);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

    }
}
