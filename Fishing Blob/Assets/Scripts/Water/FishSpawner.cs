using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private Water water;
    [SerializeField] 
    private GameObject[] fishies;
    [SerializeField]
    private int maxAmountOfFish = 20;
    [SerializeField] 
    private float waterBoundry = 0.2f;
    
    private int _curAmountOfFish;

    private float _waterHeight;
    private float _waterWidth;


    private void Start()
    {
        _waterHeight = water.GetMeshHeight();
        _waterWidth = water.GetMeshWidth();
    }
    
    private void Update()
    {
        if (_curAmountOfFish < maxAmountOfFish)
        {
            SpawnFish();    
        }
    }

    private void SpawnFish()
    {
        Vector2 randPosition = RandomPointInWater(_waterWidth, _waterHeight);
        Instantiate(fishies[Random.Range(0, fishies.Length)], randPosition, Quaternion.identity).transform.SetParent(transform, true);
        
        _curAmountOfFish++;

    }

    private Vector2 RandomPointInWater(float width, float height)
    {
        float waterStartPosX = water.transform.position.x;
        float waterStartPosY = water.transform.position.y;
        float maxWidth = waterStartPosX + width;
        return new Vector2 (Random.Range(waterStartPosX + waterBoundry, maxWidth - waterBoundry),  Random.Range(waterStartPosY + waterBoundry, height + waterStartPosY - waterBoundry));
    }
}
