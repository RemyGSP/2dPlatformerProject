using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region
    //Referencias a otros Scripts
    private Rigidbody2D rb2D;
    private PlayerCollisions playerCollisions;
    private SpriteRenderer spriteRenderer;
    private PlayerCollisions collisions;
    private PlayerSound sounds;
    private Animator animator;
    private PlayerController2 playerController;
    private BoxCollider2D objectCollider;
    private Vector3 baseColliderOffset;
    [Header("Acceleration Values")]
    [SerializeField] float runAccelAmount; [SerializeField] float runDeccelAmount; [SerializeField] float accelInAir; [SerializeField] float deccelInAir;
    float LastOnGroundTime;
    bool doConserveMomentum;
    //Velocidad
    //La direccion del input del usuario
    [SerializeField] private Sprite[] jumpSprites;
    //Esto checkea que el jugador este pulsando el espacio para poder hacer el salto mas alto
    private bool endedJumpEarly = true;
    //Indica si puede hacer un wallJump o no
    private bool canWallJump;
    private bool stopMoving = false;
    //Estas dos variables de momento no sirven de nada
    private float previousXSpeed;
    private float previousYSpeed;
    //Esta variable indica la direccion en la que esta mirando -1 es izquierda 1 es derecha, me hace las cosas mas faciles que sea un float y no un booleano
    public float lookingDirection;
    [Header("Movement Values")]
    [SerializeField] private AnimationCurve jumpForceCurve;
    [SerializeField] private float jumpTime;
    private bool isJumping;
    [SerializeField] private bool isFalling;
    [SerializeField] private float jumpTimeAccelCurve;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private Vector2 jumpDirection;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    public Vector2 playerDirection;
    public Vector2 verticalMove;
    [SerializeField] private GameObject wallJumpParticles;
    [Header("Player Visuals")]
    [SerializeField] GameObject trail;
    [SerializeField] private float playerDefaultGravity;
    [SerializeField] private float playerAugmentedGravity;
    //Limite de velocidad
    [SerializeField] GameObject leftFoot;
    [SerializeField] GameObject rightFoot;

    [SerializeField] GameObject visuals;

    #endregion Variables
    //Esto se podria quitar pero habria que encontrar otra forma de utilizar los raycast hacia al lado para entonces poder cambiar el metodo de flipear el script del personaje por otro metodo que cambie la escala de 1 a -1
    //Esto acarreara otros problemas como por ejemplo otro cambio que se tendria que hacer en el walljump para que vaya en la direccion que quieras
    //Resulta que esto no sirve de nada hay que hacer el cambio propuesto arriba
    [SerializeField] private GameObject throwingPos;
    private void Awake()
    {
        objectCollider = GetComponent<BoxCollider2D>();
        baseColliderOffset = objectCollider.offset;
        animator = visuals.GetComponent<Animator>();
        sounds = GetComponent<PlayerSound>();
        collisions = GetComponent<PlayerCollisions>();
        spriteRenderer = visuals.GetComponent<SpriteRenderer>();
        playerCollisions = GetComponent<PlayerCollisions>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController2>();
    }
    private void Update()
    {
        if (collisions.CheckGrounded())
        {
            animator.SetInteger("Jumping", 0);
        }

        else if (collisions.CheckGrounded())
        {
            animator.SetInteger("Falling", 0);

        }

        LastOnGroundTime -= Time.deltaTime;
        if (isJumping) {
            Jumping();
        }
    }
    //Aqui recogo el input del usuario para ver a donde se quiere mover
    public void OnMove(InputValue value)
    {
        playerDirection = new Vector2(value.Get<Vector2>().x, 0);
        verticalMove = new Vector2(0, value.Get<Vector2>().y);
        if (playerDirection.x != 0 && !InputCollector.instance.canThrow && GameState.CanMove || playerDirection.y != 0 && !InputCollector.instance.canThrow && GameState.CanMove)
        {
            animator.SetInteger("Running", 1);
        }
        else
        {
            animator.SetInteger("Running", 0);
        }
    }

    #region Move
    //Metodo para mover el personaje, este se ejecuta desde el update del PlayerController
    public void MovePlayer()
    {

    }
    #endregion

    //Metodo para saltar
    public void Jump()
    {
        if (InputCollector.instance.canJump && playerCollisions.CheckGrounded())
        {
            InputCollector.instance.canJump = false;
            if (!canWallJump && !isJumping)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                isJumping = true;
                //Pongo endedJumpEarly en false, esto cambiara a true cuando el jugador deje de mantener el espacio
                //endedJumpEarly = false;
                //animator.SetTrigger("onJump");
                //rb2D.AddForce((jumpDirection * jumpForce), ForceMode2D.Impulse);
            }

 


        }
        else if (InputCollector.instance.canJump && !playerCollisions.CheckGrounded() && playerCollisions.CheckSides())
        {
            WallJump();
            print("WallJump");
        }

    }

    public void Jumping()
    {
        jumpTimeAccelCurve += Time.fixedDeltaTime;
        rb2D.velocity += new Vector2(0,jumpForceCurve.Evaluate(jumpTimeAccelCurve) * jumpForce);
        animator.SetInteger("Jumping",1);
        if (jumpTimeAccelCurve >= jumpTime)
        {
            isJumping = false;
            jumpTimeAccelCurve = 0;
        }


    }

    //Esto es para que si el jugador deja de pulsar el salto caiga mas rapido, tengo que revisar el como funciona, pero de momento tira
    public void CheckIfJumpEnded()
    {
        //Este metodo me dice si esta manteniendo el espacio o el boton de salto
        if (!InputCollector.instance.HoldingJump())
        {
            endedJumpEarly = true;
        }
        //En el caso de que no este tocando el suelo y haya dejado de pulsar el espacio hace que caiga mas rapido 
        //if (!playerCollisions.CheckGrounded() && endedJumpEarly)
        //{
        //    rb2D.gravityScale = playerAugmentedGravity;
        //}
        //else
        //{
        //    rb2D.gravityScale = playerDefaultGravity;
        //}

    }

    //Esto no funciona aun me tengo que poner
    public void WallSlide()
    {
        if (playerCollisions.CheckSides() && !playerCollisions.CheckGrounded())
        {
            rb2D.drag = 0.8f;
            canWallJump = true;
                animator.SetInteger("Sliding", 1);

            
        }
        else
        {
            animator.SetInteger("Sliding", 0);
            rb2D.drag = 0.8f;
            canWallJump = false;
        }
    }


    public void WallJump()
    {
        if (canWallJump)
        {

            animator.SetInteger("Jumping", 1);
            canWallJump = false;
            stopMoving = true;
            

            if (collisions.CheckLeftSide())
            {
                Instantiate(wallJumpParticles,leftFoot.transform);
                wallJumpDirection.x = Mathf.Abs(wallJumpDirection.x);
                if (lookingDirection == -1)
                {
                    FlipCharacterRegardlessOfDirection();
                }
            }

            else if (collisions.CheckRightSide())
            {
                Instantiate(wallJumpParticles,rightFoot.transform);
                wallJumpDirection.x = Mathf.Abs(wallJumpDirection.x) * -1;
                if (lookingDirection == 1)
                {
                    FlipCharacterRegardlessOfDirection();
                }
            }

            rb2D.AddForce(wallJumpDirection * wallJumpForce, ForceMode2D.Impulse);
            stopMoving = false;
        }
    }

    private void FlipCharacter()
    {


    }

    public int GetAirSprite()
    {

        int airIndex = (int)Mathf.Clamp(
            MapSprites(rb2D.velocity.y, 15f,0f,0,3),0,2
            );
        print("AirIndex " + airIndex);
        return airIndex;
    }

    private float MapSprites(float currentVelocity, float maxYSpeed, float minYSpeed,int arrayMinIndex, int arrayMaxIndex)
    {
        int arraySize = arrayMaxIndex - arrayMinIndex;
        float speedDifference = maxYSpeed - minYSpeed;
        float speedSegments = speedDifference / arraySize;
        return currentVelocity / speedSegments;
    }
    private void FlipCharacterRegardlessOfDirection()
    {
        if (spriteRenderer.flipX) {
            spriteRenderer.transform.rotation = new Quaternion(0, 0, 0, 0);
            lookingDirection = 1;
            objectCollider.offset = new Vector2(baseColliderOffset.x, baseColliderOffset.y);
        }
        else {
            spriteRenderer.transform.rotation = new Quaternion(0, 180, 0, 0);
            lookingDirection = -1;
            objectCollider.offset = new Vector2(-baseColliderOffset.x, baseColliderOffset.y);
        }

    }

    public void LevitateOnXAxis(bool isLevitating)
    {

        if (isLevitating)
        {
            //Aqui es donde recogo la velocidad previa para darsela luego
            previousXSpeed = rb2D.velocity.x;
            rb2D.velocity = new Vector2(rb2D.velocity.x / 3, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(previousXSpeed * 2, rb2D.velocity.y);
        }

    }

    public void LevitateOnYAxis(bool isLevitating)
    {
        if (isLevitating)
        {
            previousYSpeed = rb2D.velocity.y;
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y / 3);
        }
        else
        {
            rb2D.AddForce(new Vector2(0, previousYSpeed), ForceMode2D.Impulse);
        }

    }

    public void FallThrough()
    {
        if (playerCollisions.CheckIfTraspassable() && verticalMove.y < -0.2f)
        {
            Collider2D groundCollider = playerCollisions.GetGroundCollider();
            if (groundCollider.gameObject.CompareTag("TraspassablePlatform"))
            playerCollisions.DeactivateCollider(groundCollider, true);
        }
    }

}
