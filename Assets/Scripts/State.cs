using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public abstract class State: ScriptableObject
{
    public GameObject stateGameObject;
    protected List<State> states;
    protected StateTransition[] stateTransitions;

    public virtual State CheckTransitions()
    {
        State newGameState = null;
        bool notChanged = true;
        int counter = 0;

        while (notChanged)
        {
            newGameState = stateTransitions[counter].GetExitState(stateGameObject.GetComponent<StateMachineController>());
            if (newGameState != null)
            {
                notChanged = false;
            }
            if (counter < stateTransitions.Length - 1)
            {
                counter++;
            }
            else
            {
                notChanged = false;
            }
        }

        return newGameState;
    }
    public virtual void OnEnterState()
    {
        Start();
    }
    public abstract void Start();
    public abstract void OnExitState();

    public virtual void InitializeState(GameObject newStateGameObject)
    {
        stateGameObject = newStateGameObject;
    }
    // Update is called once per frame
    public abstract void Update();

    public abstract void FixedUpdate();
}
