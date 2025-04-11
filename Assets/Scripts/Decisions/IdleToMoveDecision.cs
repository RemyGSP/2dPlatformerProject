using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDecisions/IdleToMove")]
public class IdleToMoveDecision : Decision
{
public override bool Decide(StateMachineController stateMachine)
{
        InputReceiver inputs = PlayerReferences.instance.GetInput();
        if (inputs.GetDirectionInput() == Vector2.zero)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
