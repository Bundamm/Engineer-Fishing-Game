using UnityEngine;

public interface ICastAndDestroyFloater
{
    float MaxCastPower { get; set; }
    float CastPowerIncrease { get; set; }
    void CreateFloater();
    void DestroyFloater();
}
