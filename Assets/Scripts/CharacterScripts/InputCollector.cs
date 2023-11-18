using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCollector : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputAction submitAction;
    [SerializeField] InputAction jumpAction;
    [SerializeField] InputAction throwingAction;
    public static InputCollector instance;
    public bool canJump;
    public bool canThrow;
    public bool canSubmit;

    void OnEnable()
    {
        jumpAction.Enable();
        throwingAction.Enable();
        submitAction.Enable();
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        instance = this;
        jumpAction.performed += CanJump;
        jumpAction.canceled += CanJump;
        submitAction.performed += OnSubmit;
        submitAction.canceled += OnSubmit;
        throwingAction.performed += OnThrow;
        throwingAction.canceled += OnThrow;
    }
    private void OnDisable()
    {
        throwingAction.Disable();
        jumpAction.Disable();
        submitAction.Disable();
    }


    private void CanJump(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1)
        {
            canJump = true;
        }
        else
        {
            StartCoroutine(BufferJump());
        }
    }
    public void OnSubmit(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            canSubmit = true;
        }
        else if (value.canceled)
        {
            canSubmit = false;
        }
    }

    private void OnThrow(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1)
        {
            canThrow = true;
        }
        else
        {
            canThrow = false;
        }
    }
    IEnumerator BufferJump()
    {

        yield return new WaitForSeconds(0.1f);

        canJump = false;
    }
    public bool HoldingJump()
    {
        if (jumpAction.IsPressed())
            return true;
        
        else  return false; 
    }











}
