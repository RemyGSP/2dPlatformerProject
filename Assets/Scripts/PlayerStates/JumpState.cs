using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStates/JumpState")]
public class JumpState : State
{
    Rigidbody2D rb2D;
    [SerializeField]
    PlayerCollisions collisions;
    [SerializeField]
    private AnimationCurve jumpForceCurve;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private float jumpForce;
    float jumpTimeAccelCurve;
    Animator animator;
    bool isJumping;
    public override void FixedUpdate()
    {
        jumpTimeAccelCurve += Time.fixedDeltaTime;
        rb2D.velocity += new Vector2(0, jumpForceCurve.Evaluate(jumpTimeAccelCurve) * jumpForce);
        animator.SetInteger("Jumping", 1);
        if (jumpTimeAccelCurve >= jumpTime)
        {
            stateGameObject.GetComponent<StateMachineController>().SetIsJumping(false);
        }
    }

    public override void OnExitState()
    {
        animator.SetInteger("Jumping", 0);
    }

    public override void Start()
    {
        animator = PlayerReferences.instance.GetAnimator();
        rb2D = PlayerReferences.instance.GetRigidbody();
        collisions = PlayerReferences.instance.GetPlayerCollisions();
        stateGameObject.GetComponent<StateMachineController>().SetIsJumping(true);
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
    }

    public bool IsJumping()
    {
        return isJumping;
    }


    public override void Update()
    {
    }
}
