using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReceiver : MonoBehaviour
{
    public static InputReceiver instance;

    private Vector2 moveDirection;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            print("An instance for script InputReceiver already exists");
        }
    }
    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    public Vector2 GetDirectionInput()
    {
        return moveDirection;
    }

}
