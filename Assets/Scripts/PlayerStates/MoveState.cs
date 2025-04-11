using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PlayerStates/MoveState")]

public class MoveState : State
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float runAccelAmount;
    [SerializeField]
    private float runDeccelAmount;
    [SerializeField]
    private float accelInAir;
    [SerializeField]
    private float deccelInAir;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private PlayerCollisions collisions;
    private FlipSprite spriteFlipper;
    // Start is called before the first frame update
    public override void Start()
    {
        spriteFlipper = PlayerReferences.instance.GetSpriteFlipper();
        collisions = PlayerReferences.instance.GetPlayerCollisions();
        rigidbody = PlayerReferences.instance.GetRigidbody();
        animator = PlayerReferences.instance.GetAnimator();
        animator.SetInteger("Running", 1);
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
        float targetSpeed = playerDirection.x * maxSpeed;
        targetSpeed = Mathf.Lerp(rigidbody.velocity.x, targetSpeed, 1);
        float accelerationRate;

        if (collisions.CheckGrounded())
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        else
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;


        float speedDif = targetSpeed - rigidbody.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelerationRate;
        Debug.Log(movement);
        //Convert this to a vector and apply to rigidbody
        rigidbody.velocity += (movement * Vector2.right);


        spriteFlipper.Flip(playerDirection);
    }
}
