using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class StateMachineController : MonoBehaviour
{
    [SerializeField] private State entryState;
    [SerializeField]
    private State currentState;

    private bool isJumping;
    private void Awake()
    {
        currentState = entryState;
        currentState.InitializeState(this.gameObject);
        
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
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
    public bool IsJumping()
    {
        return isJumping;
    }

    public void SetIsJumping(bool isJumping)
    {
        this.isJumping = isJumping;
    }
    public State GetCurrentState()
    {
        return currentState;
    }
}
