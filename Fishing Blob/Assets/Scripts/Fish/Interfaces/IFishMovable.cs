using UnityEngine;

public interface IFishMovable
{
    Rigidbody2D fishRB { get; set; }

    void MoveFish(Vector2 path);
    
    void MoveFishWithoutRotating(Vector2 velocity);

    Vector2 GetRandomDirectionInWater();

}
