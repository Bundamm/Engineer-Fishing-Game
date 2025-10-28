using UnityEngine;

public interface IFishAndFloaterPositionAndRotation
{
    Vector2 GetPathToFloater();

    Vector2 GetPathToStartPositionOfFish();
    float GetAngleBetweenFishAndFloater();

    Vector2 GetFloaterPosition();
    Vector2 GetFishPosition();
}
