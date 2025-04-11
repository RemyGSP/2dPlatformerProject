using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PlayerStates/IdleState")]

public class IdleState : State
{
    private Rigidbody2D body = null;
    public override void FixedUpdate()
    {
        Debug.Log(body);
        if (body != null)
        {
            Vector2 newSpeed = new Vector2(Vector2.Lerp(body.velocity, Vector2.zero, 0.4f).x, body.velocity.y);
            body.velocity = newSpeed;
            
        }
    }

    public override void OnEnterState()
    {
        base.OnEnterState();    
        body = PlayerReferences.instance.GetRigidbody();

    }
    public override void OnExitState()
    {
    }

    public override void Start()
    {
        body = PlayerReferences.instance.GetRigidbody();
    }

    public override void Update()
    {
    }


}
