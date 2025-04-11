using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDecisions/JumpDecision")]

public class JumpDecision : Decision
{
    public override bool Decide(StateMachineController stateMachine)
    {
        if (PlayerReferences.instance.GetInput().IsPressingJump() && PlayerReferences.instance.GetPlayerCollisions().CheckGrounded())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
