using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDecisions/StopJumpDecision")]

public class StopJumpDecision : Decision
{
    public override bool Decide(StateMachineController stateMachine)
    {
        if (stateMachine.IsJumping())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
