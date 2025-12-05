using UnityEngine;
public class CasterThrowingState : CasterState
{
    public CasterThrowingState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Caster Throwing State");
        Debug.Log(Caster);
        
        // Floater initialization and casting
        Caster.CreateFloater();
        Caster.castVector = new Vector2(Caster.Rod.castPower / 2, Caster.Rod.castPower / 3);
        Caster.currentFloater.GetComponent<Rigidbody2D>().AddForce(Caster.castVector,  ForceMode2D.Impulse);
        Caster.lineSpawner.InitLine(Caster.currentFloater);
        Caster.lineSpawner.SetLineActive(true);
        
        // Camera change to floater
        Caster.cameraManager.ChangeCamera(Caster.floaterCamera, Caster.currentFloater.transform);
        Fsm.ChangeState(Caster.WaitingForReturnState);
    }

    public override void ExitState()
    {
        base.ExitState();
        Rod.castPower = 0f;
    }
}
