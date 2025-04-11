using UnityEngine;
[CreateAssetMenu(menuName = "PlayerDecisions/MoveToIdle")]
public class MoveToIdleDecision : Decision
{
public override bool Decide(StateMachineController stateMachine)
    {
        InputReceiver inputs = PlayerReferences.instance.GetInput();
        if (inputs.GetDirectionInput() == Vector2.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
