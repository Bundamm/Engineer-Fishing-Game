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
    

    

    public float MovementSpeed => movementSpeed;
    public float TimeUntilSpooked => timeUntilSpooked;
    public int MaxAmountOfBites => maxAmountOfBites;
    public float WaitAtStartMaxTime => waitAtStartMaxTime;
    public float WaitUntilIdleTime => waitUntilIdleTime;
    public float DirectionVectorLength => directionVectorLength;
    public float WaterBorder => waterBorder;
}
