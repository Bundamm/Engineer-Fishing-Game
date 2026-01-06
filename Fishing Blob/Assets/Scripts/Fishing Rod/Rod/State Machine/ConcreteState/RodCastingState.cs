using System.Collections;
using UnityEngine;

public class RodCastingState : RodState
{
    public RodCastingState(Rod rod, RodStateMachine fsm) : base(rod, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Rotating To Throw State");

    }
    
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Rod.StartCoroutine(RotateBack());
        
        if (RodRotator.GetIsRotated())
        {
            if (Rod.playerTransform.localScale.x < 0)
            {
                Rod.castPower = -Rod.castPower;
                Debug.Log(Rod.castPower);
            }

            AudioManager.Instance.PlaySound(AudioManager.SoundType.Cast, Rod.RodSource);
            Fsm.ChangeState(Rod.ThrowingState);
        }

    }

    private IEnumerator RotateBack()
    {
        if (Rod.castPower > Mathf.Epsilon)
        {
            while (Quaternion.Angle(RodRotator.GetCurrentRotation(), RodRotator.GetStartRotation()) > 5f)
            {
                yield return new WaitForEndOfFrame();
                RodRotator.RotateRod(RodRotator.GetStartRotation(), Rod.castRotationSpeed);
            }
        }
    }
}
