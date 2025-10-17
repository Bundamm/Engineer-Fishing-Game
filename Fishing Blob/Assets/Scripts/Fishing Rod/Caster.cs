using System.Collections;
using UnityEngine;

public class Caster : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField]
    InputHandler inputHandler;
    [SerializeField]
    private RodRotator rod;

    [Header("Casting")]
    [SerializeField]
    private float maxCastPower = 5f;
    [SerializeField] 
    private float castPowerIncrease = 1f;
    [SerializeField]
    private GameObject floaterPrefab;
    private GameObject _currentFloater;
    [SerializeField] 
    private Floater floaterBehaviour;
    
    [SerializeField] 
    private float defaultCastSpeed = 100f;
    private float _castPower;
    private Vector2 _cast;
    private bool _casted;
    private bool _casting;

    [Header("Rod Rotation")] 
    [SerializeField]
    private Quaternion targetRotation;
    [SerializeField] 
    private float firstRotationSpeed = 1;
    [SerializeField]
    private float secondRotationSpeed = 2;

    [Header("Fishing Line")] 
    [SerializeField]
    private LineSpawner lineSpawner;

    
    private void Update()
    {
        CastHandler();
    }

    private void CastHandler()
    {
        if (inputHandler.GetCastValue())
        {
            _casting = true;
            ResetCast();
            ChargeCast();
        }
        else if (_casting)
        {
            StartCoroutine(ReleaseCast());
            _casting = false;
        }
    }

    private void ResetCast()
    {
        _casted = false;
        rod.SetIsRotated(false);
    }

    private void ChargeCast()
    {
        rod.RotateRod(targetRotation,firstRotationSpeed);
        if (rod.GetIsRotated())
        {
            CalculateCastPower();
        }
    }

    private IEnumerator ReleaseCast()
    {
        if (_castPower > Mathf.Epsilon && !_casted)
        {
            while (Quaternion.Angle(rod.GetCurrentRotation(),  rod.GetStartRotation()) > 5f)
            {
                yield return new WaitForEndOfFrame();
                rod.RotateRod(rod.GetStartRotation(), secondRotationSpeed);
            }
            if (rod.GetIsRotated())
            { 
                CastFloater(_castPower);
            }
            _casted = true;
            _castPower = 0f;
        }
    }
    
    
    private void CalculateCastPower()
    {
        if (_castPower <= maxCastPower)
        {
            _castPower += castPowerIncrease * Time.deltaTime;
            // Debug.Log(_castPower);
        }
    }
    

    // Spawns and adds power to the floater
    private void CastFloater(float power)
    {
        if (_currentFloater != null)
        {
            Destroy(_currentFloater);
        }
        floaterBehaviour.SetIsInWater(false);
        _cast = new Vector2(power / 2, power / 3);
        _currentFloater = Instantiate(floaterPrefab, transform.position, Quaternion.identity);
        _currentFloater.GetComponent<Rigidbody2D>().AddForce(_cast, ForceMode2D.Impulse);
        lineSpawner.InitLine(_currentFloater);
        lineSpawner.SetLineActive(true);
        
    }
}
