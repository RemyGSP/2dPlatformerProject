using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Movement movement;
    private InputCollector inputCollector;
    private PlayerCollisions playerCollisions;
    private Skill skill;
    private bool playerStatic;
    void Start()
    {
        playerCollisions = GetComponent<PlayerCollisions>();
        inputCollector = GetComponent<InputCollector>();
        movement = GetComponent<Movement>();
        skill = GetComponent<Skill>();
    }


    // Update is called once per frame
    void Update()
    {
        if (inputCollector.CheckIfJump() && playerCollisions.CheckGrounded())
        {
            movement.Jump();
        }
        else if (inputCollector.CheckIfJump() && !playerCollisions.CheckGrounded() && playerCollisions.CheckSides())
            movement.WallJump();
        else
            movement.CheckIfJumpEnded();
        movement.FallThrough();
        skill.UseSkill();


    }

    private void FixedUpdate()
    {
        if (!playerStatic)
        {
            movement.MovePlayer();
            if (!playerStatic)
            {

                movement.WallSlide();

            }
        }
    }
    public void StopMoving()
    {
        playerStatic = true;
    }

    public void ResumeMoving()
    {
        playerStatic = false;
    }

}
