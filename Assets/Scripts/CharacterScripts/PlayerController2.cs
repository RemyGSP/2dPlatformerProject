using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Movement movement;
    private InputCollector inputCollector;
    private PlayerCollisions playerCollisions;
    private Skill skill;
    
    void Start()
    {
        GameState.CanMove = true;
        GameState.canThrow = true;
        playerCollisions = GetComponent<PlayerCollisions>();
        inputCollector = GetComponent<InputCollector>();
        movement = GetComponent<Movement>();
        skill = GetComponent<Skill>();
    }


    // Update is called once per frame
    void Update()
    {
        if (GameState.canThrow)
        skill.UseSkill();
        movement.WallSlide();


    }

    private void FixedUpdate()
    {
        if (GameState.CanMove)
        {
            movement.MovePlayer();
            movement.Jump();
            movement.CheckIfJumpEnded();
            movement.FallThrough();
        }


    }
    public void StopMoving()
    {
        GameState.CanMove = true;
    }

    public void ResumeMoving()
    {
        GameState.CanMove = false;
    }

}
