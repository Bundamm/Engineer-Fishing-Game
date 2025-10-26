﻿using System.Collections;
using UnityEngine;

public class RodRotatingToThrowState : RodState
{
    public RodRotatingToThrowState(Rod rod, RodStateMachine fsm) : base(rod, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Rotating To Throw State");

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Rod.StartCoroutine(RotateBack());
        if (RodRotator.GetIsRotated())
        {
            Fsm.ChangeState(Rod.ToCasterReleaseFloaterState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Rod.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
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
