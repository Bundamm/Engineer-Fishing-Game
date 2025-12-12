using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float startCameraPosX;
    private float startLayerX;
    [SerializeField] 
    private Transform targetCamera;
    [SerializeField] 
    [Range(0f, 1f)]
    private float parallaxAmount;
    
    void Start()
    {
        startCameraPosX = targetCamera.position.x;
        startLayerX = transform.position.x;
    }
    
    void LateUpdate()
    {
        float distanceMoved = targetCamera.position.x - startCameraPosX;
        float newX = startLayerX + (distanceMoved * parallaxAmount);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

    }
}
