using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class StateMachineController : MonoBehaviour
{
    [SerializeField] private States entryState;
    private State currentState;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }
    
    private void LateUpdate()
    {
        State newState = currentState.CheckTransitions();
        if (newState is not null)
        {
            ChangeState(newState);
        }
    }
    public void ChangeState(State newState)
    {
        currentState.OnExitState();
        currentState = Instantiate(newState);
        currentState.InitializeState(this.gameObject);
        currentState.OnEnterState();
    }
}
