using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableBehaviour : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    [SerializeField] private float rotationAmount;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigidBody2D.rotation += rotationAmount;
    }
}
