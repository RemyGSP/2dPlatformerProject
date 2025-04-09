using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

[System.Serializable]
public class StateTransition
{
    [SerializeField] Decision decisionToBeMade;
    [SerializeField] State onDecisionTrueExitState;
    [SerializeField] State onDecisionFalseExitState;

    public State GetExitState(StateMachineController stateMachine)
    {
        //Debug.Log(decisionToBeMade.Decide(stateMachine) ? onDecisionTrueExitState : onDecisionFalseExitState );  
        return decisionToBeMade.Decide(stateMachine) ? onDecisionTrueExitState : onDecisionFalseExitState;
    }
}
