using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    #region Fish Serializable Properties
    [Header("Fish Properties")]
    [SerializeField] 
    private GameObject[] fishiesToSpawn;
    [SerializeField]
    private int maxAmountOfFish = 20;
    [SerializeField] 
    private float waterBoundry = 0.2f;
    [SerializeField] 
    private Transform fishContainer;
    [SerializeField] 
    private float fishDepth = 5f;
    [SerializeField]
    [Range(1,6)]
    private int fishReplaceTick;
    #endregion
    
    #region Other Objects
    [Header("Other Objects")]
    [SerializeField]
    private Water water;
    [SerializeField]
    private TimeManager timeManager;
    #endregion
    
    #region Helpers
    private List<GameObject> spawnedFish;
    private List<Fish> fishiesToSpawnObjects;
    private int _curAmountOfFish;
    private float _waterHeight;
    private float _waterWidth;
    #endregion

    private void Awake()
    {
        spawnedFish = new List<GameObject>();
        fishiesToSpawnObjects = new List<Fish>();
        for (int i = 0; i < fishiesToSpawn.Length; i++)
        {
            fishiesToSpawnObjects.Add(fishiesToSpawn[i].GetComponent<Fish>());
        }
    }
    
    private void Start()
    {
        _waterHeight = water.GetMeshHeight();
        _waterWidth = water.GetMeshWidth();
    }
    
    private void Update()
    {
        SpawnMenuGame();
    }

    private void SpawnMenuGame()
    {
        if (timeManager != null)
        {
            if (timeManager.TimerPaused) return;
            if (_curAmountOfFish < maxAmountOfFish && timeManager.Fsm.IsInState(timeManager.DayActiveState))
            {
                SpawnFish();    
            }
        }
        else
        {
            if (_curAmountOfFish < maxAmountOfFish)
            {
                SpawnFish();
            }
        }
    }

    private void SpawnFish()
    {
        Vector2 randPosition = RandomPointInWater(_waterWidth, _waterHeight);
        Vector3 spawnPosition = new Vector3(randPosition.x, randPosition.y, fishDepth);
        GameObject newFish = Instantiate(fishiesToSpawn[Random.Range(0, fishiesToSpawn.Length)], spawnPosition, Quaternion.identity);
        newFish.transform.SetParent(fishContainer, true);
        newFish.GetComponent<Fish>().FishSpawner = this;
        //TODO: NEED TO GET ALL FISH CURRENTLY IN THE WATER
        spawnedFish.Add(newFish);
        _curAmountOfFish++;
    }
    
    private Vector2 RandomPointInWater(float width, float height)
    {
        float waterStartPosX = water.transform.position.x;
        float waterStartPosY = water.transform.position.y;
        float maxWidth = waterStartPosX + width;
        return new Vector2 (Random.Range(waterStartPosX + waterBoundry, maxWidth - waterBoundry),  Random.Range(waterStartPosY + waterBoundry, height + waterStartPosY - waterBoundry));
    }
    

    public void DespawnFish(GameObject fish)
    {
        spawnedFish.Remove(fish);
        Destroy(fish);
    }

    public void DespawnAllFish()
    {
        for (int i = spawnedFish.Count - 1; i >= 0; i--)
        {
            Debug.Log(spawnedFish[i]);
            if (spawnedFish[i] != null)
            {
                spawnedFish[i].GetComponent<Fish>().FishSpawner = null;
                DespawnFish(spawnedFish[i]);
            }
            else
            {
                spawnedFish.RemoveAt(i);
            }
        }
        _curAmountOfFish = 0;
    }
    
    

    public void FeedFish(FishTypeEnum fishType)
    {
        GameObject fishToReplaceWith = new GameObject();
        GameObject tempFish = new GameObject();
        for (int i = 0; i < fishiesToSpawn.Length; i++)
        {
            if (fishiesToSpawnObjects[i].fishType.FishTypeEnum == fishType)
            {
                fishToReplaceWith = fishiesToSpawn[i];
            }
        }
        for (int i = 0; i < spawnedFish.Count; i++)
        {
            if (i % fishReplaceTick == 0)
            {
                tempFish =  spawnedFish[i];
                spawnedFish[i] = Instantiate(fishToReplaceWith, spawnedFish[i].transform.position, Quaternion.identity);
                DespawnFish(tempFish);
            }
        }
    }

}
