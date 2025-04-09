using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float runAccelAmount;
    private float runDeccelAmount;
    private float accelInAir;
    private float deccelInAir;
    private Animator animator;
    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    public override void Start()
    {
        rigidbody = PlayerReferences.instance.GetRigidbody();
        animator.SetInteger("Running", 1);
        animator = PlayerReferences.instance.GetAnimator();
    }

    // Update is called once per frame
    public override void Update()
    {
 

    }

    public override void OnExitState()
    {
        animator.SetInteger("Running",0);
    }

    public override void FixedUpdate()
    {
        Vector2 playerDirection = InputReceiver.instance.GetDirectionInput();
        #region Experimental
        float targetSpeed = playerDirection.x * maxSpeed;
        targetSpeed = Mathf.Lerp(rb2D.velocity.x, targetSpeed, 1);
        float accelerationRate;

        if (PlayerCollisions)
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        else
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;


        float speedDif = targetSpeed - rigidbody.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelerationRate;

        //Convert this to a vector and apply to rigidbody
        rigidbody.velocity = (movement * Vector2.right);
        if (collisions.CheckGrounded() && playerDirection != Vector2.zero) playerController.runningParticles.SetActive(true);
        else playerController.runningParticles.SetActive(false);
        //if () sounds.PlayFootstepSound();

        FlipCharacter();
    }
}
