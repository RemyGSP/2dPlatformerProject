using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputCollector : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputAction submitAction;
    [SerializeField] InputAction jumpAction;
    [SerializeField] InputAction throwingAction;
    [SerializeField] InputAction respawnAction;
    [SerializeField] InputAction cancelTpAction;
    [SerializeField] InputAction menuAction;
    public UnityEvent respawn;
    public UnityEvent cancel;
    public UnityEvent menu;
    public static InputCollector instance;
    public bool canJump;
    public bool canThrow;
    public bool canSubmit;

    void OnEnable()
    {
        jumpAction.Enable();
        throwingAction.Enable();
        submitAction.Enable();
        respawnAction.Enable();
        cancelTpAction.Enable();
        menuAction.Enable();
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
        submitAction.performed += OnSubmiting;
        submitAction.canceled += OnSubmiting;
        throwingAction.performed += OnThrowing;
        throwingAction.canceled += OnThrowing;
        respawnAction.performed += OnRespawn;
        cancelTpAction.performed += OnCancelTP;
        menuAction.performed += OnMenu;
    }
    private void OnDisable()
    {
        throwingAction.Disable();
        jumpAction.Disable();
        submitAction.Disable();
        respawnAction.Disable(); 
        cancelTpAction.Disable();
        menuAction.Disable();
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
    public void OnSubmiting(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1)
        {
            canSubmit = true;
        }
        else
        {
            canSubmit = false;
        }
    }

    private void OnThrowing(InputAction.CallbackContext value)
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

    public void OnRespawn(InputAction.CallbackContext value)
    {
        respawn.Invoke();
    }

    public void OnCancelTP(InputAction.CallbackContext value)
    {
        cancel.Invoke();
    }

    public void OnMenu(InputAction.CallbackContext value)
    {
        menu.Invoke();
    }
}
