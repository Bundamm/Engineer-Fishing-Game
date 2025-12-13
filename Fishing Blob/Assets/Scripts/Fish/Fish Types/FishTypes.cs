using UnityEngine;

[CreateAssetMenu(fileName = "FishTypes", menuName = "Scriptable Objects/FishTypes")]
public class FishTypes : ScriptableObject
{
    [SerializeField] 
    private float movementSpeed;
    [SerializeField] 
    private float directionVectorLength;
    [SerializeField] 
    private float waterBorder;
    [SerializeField]
    private float timeUntilSpooked;
    [SerializeField]
    private int maxAmountOfBites;
    [SerializeField]
    private float waitAtStartMaxTime;
    [SerializeField]
    private float waitUntilIdleTime;
    [SerializeField] 
    private FishTypeEnum fishTypeEnum;
    [SerializeField]
    private float fishValue;
    [SerializeField] 
    private float constantFishValue;
    [SerializeField] 
    private float maxFishValue;
    

    

    public float MovementSpeed => movementSpeed;
    public float TimeUntilSpooked => timeUntilSpooked;
    public int MaxAmountOfBites => maxAmountOfBites;
    public float WaitAtStartMaxTime => waitAtStartMaxTime;
    public float WaitUntilIdleTime => waitUntilIdleTime;
    public float DirectionVectorLength => directionVectorLength;
    public float WaterBorder => waterBorder;
    public FishTypeEnum FishTypeEnum => fishTypeEnum;
    public decimal FishValue
    {
        get => (decimal)fishValue;
        set => fishValue = Mathf.Clamp((float)value, 15f, maxFishValue);
    }
    public decimal ConstantFishValue => (decimal)constantFishValue;
}
public enum FishTypeEnum
{
    Ablat,
    Bluegill,
    Crucian,
    Goldfish,
    Perch,
    Pike
}

