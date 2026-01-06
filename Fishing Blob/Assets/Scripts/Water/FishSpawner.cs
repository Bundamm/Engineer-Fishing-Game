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
    private List<GameObject> _spawnedFish;
    private List<Fish> _fishiesToSpawnObjects;
    private int _curAmountOfFish;
    private float _waterHeight;
    private float _waterWidth;
    #endregion

    private void Awake()
    {
        _spawnedFish = new List<GameObject>();
        _fishiesToSpawnObjects = new List<Fish>();
        for (int i = 0; i < fishiesToSpawn.Length; i++)
        {
            _fishiesToSpawnObjects.Add(fishiesToSpawn[i].GetComponent<Fish>());
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
        _spawnedFish.Add(newFish);
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
        _spawnedFish.Remove(fish);
        Destroy(fish);
    }

    public void DespawnAllFish()
    {
        for (int i = _spawnedFish.Count - 1; i >= 0; i--)
        {
            Debug.Log(_spawnedFish[i]);
            if (_spawnedFish[i] != null)
            {
                _spawnedFish[i].GetComponent<Fish>().FishSpawner = null;
                DespawnFish(_spawnedFish[i]);
            }
            else
            {
                _spawnedFish.RemoveAt(i);
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
            if (_fishiesToSpawnObjects[i].fishType.FishTypeEnum == fishType)
            {
                fishToReplaceWith = fishiesToSpawn[i];
            }
        }
        for (int i = 0; i < _spawnedFish.Count; i++)
        {
            if (i % fishReplaceTick == 0)
            {
                tempFish =  _spawnedFish[i];
                _spawnedFish[i] = Instantiate(fishToReplaceWith, _spawnedFish[i].transform.position, Quaternion.identity);
                DespawnFish(tempFish);
            }
        }
    }

}
