using System.Collections;
using System.Collections.Generic;
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
    private BoxCollider2D objectCollider;
    private Vector3 baseColliderOffset;
    [Header("Acceleration Values")]
    [SerializeField] float runAccelAmount; [SerializeField] float runDeccelAmount; [SerializeField] float accelInAir; [SerializeField] float deccelInAir;
    float LastOnGroundTime;
    bool doConserveMomentum;
    //Velocidad
    //La direccion del input del usuario

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
    [SerializeField] private float maxSpeed;
    [SerializeField] private Vector2 wallJumpDirection;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private Vector2 jumpDirection;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    public Vector2 playerDirection;
    public Vector2 verticalMove;

    [Header("Player Visuals")]
    [SerializeField] GameObject trail;
    [SerializeField] private float playerDefaultGravity;
    [SerializeField] private float playerAugmentedGravity;
    //Limite de velocidad


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
    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
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
        //Esto es para poder hacer que el personaje deje de moverse en el caso de que sea necesario
        if (!stopMoving)
        {
            #region Experimental
            float targetSpeed = playerDirection.x * maxSpeed;
            targetSpeed = Mathf.Lerp(rb2D.velocity.x, targetSpeed, 1);
            float accelerationRate;

            if (LastOnGroundTime > 0)
                accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
            else
                accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;

            if (doConserveMomentum && Mathf.Abs(rb2D.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb2D.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
            {
                accelerationRate = 0;
            }
            float speedDif = targetSpeed - rb2D.velocity.x;
            //Calculate force along x-axis to apply to thr player

            float movement = speedDif * accelerationRate;

            //Convert this to a vector and apply to rigidbody
            rb2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
            FlipCharacter();

            #endregion
        }
    }
    #endregion

    //Metodo para saltar
    public void Jump()
    {
        if (InputCollector.instance.canJump && playerCollisions.CheckGrounded())
        {
            InputCollector.instance.canJump = false;
            //Esto es para que si esta en un muro no haga el salto normal
            if (!canWallJump)
            {
                //Primero reproduzo el sonido del salto
                sounds.PlayJumpSound();
                //Pongo endedJumpEarly en false, esto cambiara a true cuando el jugador deje de mantener el espacio
                endedJumpEarly = false;
                //Aplico la fuerza del salto
                rb2D.AddForce((jumpDirection * jumpForce), ForceMode2D.Impulse);
            }


        }
        else if (InputCollector.instance.canJump && !playerCollisions.CheckGrounded() && playerCollisions.CheckSides())
            WallJump();

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
        if (!playerCollisions.CheckGrounded() && endedJumpEarly)
        {
            rb2D.gravityScale = playerAugmentedGravity;
        }
        else
        {
            rb2D.gravityScale = playerDefaultGravity;
        }

    }

    //Esto no funciona aun me tengo que poner
    public void WallSlide()
    {
        if (playerCollisions.CheckSides() && !playerCollisions.CheckGrounded())
        {
            rb2D.drag = 0.8f;
            canWallJump = true;
        }
        else
        {
            rb2D.drag = 0.8f;
            canWallJump = false;
        }
    }

    /*Hay que cambiar esto, hay una pequeña ventana de tiempo en la que el jugador si intenta hacer el wallJump en un muro situado a su derecha y mirando hacia la derecha y justo despues de saltar
     * en un timing preciso pulsa en la otra direccion el salto que se hace no es el correcto, se que causa el bug, es debido a que el jugador se gira utilizando la escala, hago que la escala sea -1 o 1
     * entonces si.
     * */
    public void WallJump()
    {
        //Primero miro si puede hacer el salto
        if (canWallJump)
        {
            sounds.PlayJumpSound();
            rb2D.gravityScale = 1f;
            canWallJump = false;
            stopMoving = true;

            if (collisions.CheckLeftSide())
            {
                wallJumpDirection.x = Mathf.Abs(wallJumpDirection.x);
                if (lookingDirection == -1)
                {
                    FlipCharacterRegardlessOfDirection();
                }
            }

            //Hago lo mismo pero con el lado derecho
            else if (collisions.CheckRightSide())
            {
                //Aqui hago que la direccion x de el wallJump siempre sea negativa dado que si el muro esta a la derecho yo quiero que vaya a la izquierda
                wallJumpDirection.x = Mathf.Abs(wallJumpDirection.x) * -1;
                //Aqui hago que se de la vuelta el sprite puramente estetico, muy posiblemente lo pueda sustituir por una animacion de wallJump
                if (lookingDirection == 1)
                {
                    FlipCharacterRegardlessOfDirection();
                }
            }

            //Aqui reseteo la velocidad vertical para luego hacer el salto
            rb2D.velocity = new Vector2(0f, 0f);
            //Hago el salto
            rb2D.AddForce(wallJumpDirection * wallJumpForce, ForceMode2D.Impulse);
            //Le devuelvo el movimiento del personaje al jugador
            stopMoving = false;
        }
    }

    //Gira el sprite de el personaje y me cambia la variable looking direction para que me diga hacia que lado mira
    private void FlipCharacter()
    {
        if (playerDirection.x > 0) 
        { spriteRenderer.flipX = false; 
            lookingDirection = 1; 
            throwingPos.transform.localPosition = new Vector2(Mathf.Abs(throwingPos.transform.localPosition.x), throwingPos.transform.localPosition.y);
            objectCollider.offset = new Vector2(baseColliderOffset.x, baseColliderOffset.y);

        }
        else if (playerDirection.x < 0)
        {
            spriteRenderer.flipX = true;
            lookingDirection = -1;
            throwingPos.transform.localPosition = new Vector2(throwingPos.transform.localPosition.x < 0 ? throwingPos.transform.localPosition.x : -throwingPos.transform.localPosition.x, throwingPos.transform.localPosition.y);
            objectCollider.offset = new Vector2(-baseColliderOffset.x, baseColliderOffset.y);

        }

    }

    //Esto quiza se podria sustituir por otro FlipCharacter con otros parametros
    private void FlipCharacterRegardlessOfDirection()
    {
        if (spriteRenderer.flipX) { spriteRenderer.flipX = false; lookingDirection = 1; objectCollider.offset = new Vector2(baseColliderOffset.x, baseColliderOffset.y);
        }
        else { spriteRenderer.flipX = true; lookingDirection = -1; objectCollider.offset = new Vector2(-baseColliderOffset.x, baseColliderOffset.y);
        }

    }

    //Esto hace que vaya mas lento como si levitara en el eje X, no funciona bien me lo he de revisar no hace lo que quiero, esto deberia hacer que se ralentizara pero luego volviera a la misma velocidad cuando acabe, no lo hace
    //El parametro del metodo es si levita o no, true levita false deja de levitar
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

    //Esto hace que vaya mas lento como si levitara en el eje Y, no funciona bien me lo he de revisar no hace lo que quiero, esto deberia hacer que se ralentizara pero luego volviera a la misma velocidad cuando acabe, no lo hace
    //El parametro del metodo es si levita o no, true levita false deja de levitar
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
            playerCollisions.DeactivateCollider(playerCollisions.GetGroundCollider(), true);
        }
    }

}
