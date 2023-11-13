using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCollector : MonoBehaviour
{
    [SerializeField] InputAction jumpAction;
    [SerializeField] InputAction throwingAction;
    void Start()
    {
        jumpAction.Enable();
        throwingAction.Enable();
    }

    public bool CheckIfJump()
    {
        bool jump = false;
        if (jumpAction.WasPressedThisFrame())
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        return jump;
    }


    public bool HoldingJump()
    {
        if (jumpAction.IsPressed())
            return true;
        
        else  return false; 
    }

    public bool IsThrowing()
    {
        bool throwing = false;
        if (throwingAction.WasPressedThisFrame()) { throwing = true; }
        return throwing;
    }

    public bool IsHoldingThrowing()
    {
        bool holding = false;
        if (throwingAction.IsPressed()) { holding = true; }

        return holding;
    }




}
