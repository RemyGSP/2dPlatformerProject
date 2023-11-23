 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Skill : MonoBehaviour
{
    [SerializeField] GameObject visuals;
    private Animator anims;
    #region
    [Header("Inputs")]
    [SerializeField] private InputAction facingAt;
    //La fuerza con la que se tira el projectil
    [Header("Throw values")]
    [SerializeField] private Vector2 throwDirection;
    [SerializeField] private float throwForce;
    //Un prefab del projectil
    [SerializeField] private GameObject objectToThrow;
    //Desde donde se tira el objeto esto podria ser la pierna el brazo o lo que sea
    [SerializeField] private Transform objectSpawnPoint;
    private Vector2 defaultThrow;

    [Header("Line Renderer")]
    //Esta es la cantidad de puntos que tiene la linea de la trajectoria
    [SerializeField] private int linePoints = 25;
    //Este es el tiempo entre puntos
    [SerializeField] private float timeBetweenPoints = 0.25f;
    //Esto es un lineRenderer que esta en el jugador para poder dibujar la linea con la trajectoria del projectil para que el jugador vea donde va a ir
    private LineRenderer lineRenderer;
    [Header("Timers")]
    //Un cooldown que le he puesto al tp para que no se vaya de manos
    [SerializeField] private float tpCooldown;
    //Lo mismo, un cooldown para lanzar el objeto porque sino hay interacciones no deseadas
    [SerializeField] private float throwingCooldown;
    [SerializeField] private float cooldownDuration;
    [SerializeField] private float inputGraceTimerLimit;

    [Header("Gravity Values")]
    //Estos son unos valores que cambias en el editor, tienes que poner tres valores, el fallClap, la minFallSpeed y la maxFallSpeed, esto ralentiza la gravedad, o la puede aumentar como tu quieras
    [SerializeField] float sloMoGravity;
    //Aqui guardo los valores de la gravedad antes de cambiarlos para poder devolverlo a su estado normal una vez acabado el SloMo
    [SerializeField] float preSloMoGravity = 1;
    //Esto sirve para comprobar el cooldown del tp
    private float tpTimer;
    [Header("Graphic Feedback")]
    [SerializeField] GameObject tpParticles;
    [SerializeField] GameObject tpFeedbackBorder;
    //Esta variable es puramente estetica es para que el objeto lanzado vaya dando vueltas y se vea mejor
    [SerializeField] private float objectRotation;
    //ESto sirve para comprobar el cooldown del lanzamiento
    private float throwTimer;
    //Esto comprueba si el boton de lanzar ha sido pulsado
    private bool buttonPressed;
    private float inputGraceTimer;
    private Vector2 inputDirection;
    BoxCollider2D boxCollider;
    private bool throwActionInitiated;
    private float playerHeight;
    private float playerWidth;
    private bool canChangeDirection;

    [Header("CollisionMask")]
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb2D;
    //Referencias a otros scripts
    private PlayerSound sounds;
    private PlayerController2 playerController;
    private PlayerCollisions playerCollisions;
    private InputCollector inputCollector;
    public GameObject currentThrowable;
    private Movement movement;

    #endregion Variables
    private void Start()
    {
        anims = visuals.GetComponent<Animator>();
        canChangeDirection = true;
        boxCollider = GetComponent<BoxCollider2D>();
        playerHeight = boxCollider.size.y;
        playerWidth = boxCollider.size.x;
        sounds = GetComponent<PlayerSound>();
        lineRenderer = GetComponent<LineRenderer>();
        playerCollisions = GetComponent<PlayerCollisions>();
        inputGraceTimer = 0;
        facingAt.performed += OnFacingAt;
        movement = GetComponent<Movement>();
        playerController = GetComponent<PlayerController2>();
        rb2D = GetComponent<Rigidbody2D>();
        inputCollector = GetComponent<InputCollector>();
        /*lo establezco en null porque tengo un if que comprueba que el currentThrowable sea null, se podria 
         * cambiar, si se quita se rompe el sistema de lanzamiento y tp*/
        currentThrowable = null;

        //Esto es un quality of life para que el jugador pueda tirar el projectil inmediatamente la primera vez que se carga el jugador
        //esto esta hecho con la intencion de dar zero downtime al jugador
        throwTimer = throwingCooldown;
    }

    private void LateUpdate()
    {
        throwDirection = inputDirection;
    }
    //Esta variable es llamada en el update del PlayerController y basicamente hace las comprobaciones para utilizar la habilidad
    public void UseSkill()
    {
        if (throwDirection.x == 0 && throwDirection.y == 0) throwDirection = new Vector2(movement.playerDirection.x , movement.verticalMove.y);
        //Esto comprueba si se esta manteniendo el boton pulsado, si el cooldown esta cumplido y si no hay ningun objeto ya generado
        //esto lo hago para que mientras se mantenga pulsado el boton se muestre la trajectoria del projectil y se aplique el slomo
        if (inputCollector.canThrow && throwTimer > throwingCooldown && currentThrowable == null)
        {
            anims.SetInteger("startThrow", 1);
            buttonPressed = true;
            ShowTrajectory();
            facingAt.Enable();
            movement.LevitateOnXAxis(true);
            movement.LevitateOnYAxis(true);
            GameState.CanMove = false;
            SlowMo();
        }
        if (inputCollector.canThrow && currentThrowable != null && tpTimer > tpCooldown)
        {
            GameObject a = Instantiate(tpParticles);
            a.transform.position = this.transform.position;
            GameObject b = Instantiate(tpParticles);
            b.transform.position = currentThrowable.transform.position;
            TeleportToThrowable();

            rb2D.gravityScale = 2;
            tpTimer = 0;
            throwTimer = 0;
        }
        //Esto se cumple en el momento en el que se deja de mantener el boton y lo que hace es parar el sloMo, lanzar el projectil y esconder la trajectoria
        else if (!inputCollector.canThrow && buttonPressed && currentThrowable == null)
        {
            anims.SetInteger("startThrow", 0);
            anims.SetTrigger("endThrow");
            ThrowObject();
            throwDirection = Vector2.zero;
            sounds.PlayThrowSound();
            movement.LevitateOnXAxis(false);
            movement.LevitateOnYAxis(false);
            GameState.CanMove = true;
            StopSloMo();
            HideTrajectory();
            facingAt.Disable();
            buttonPressed = false;
        }
        //Aqui voy a�adiendo a los timers para poder utilizar correctamente los cooldowns
        throwTimer += Time.deltaTime;
        tpTimer += Time.deltaTime;


    }

    //Esto basicamente lanza el objeto, lo instancia, y le da una velocidad
    private void ThrowObject()
    {
        currentThrowable = Instantiate(objectToThrow);
        currentThrowable.transform.position = objectSpawnPoint.position;
        currentThrowable.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwDirection.x, throwDirection.y) * throwForce, ForceMode2D.Impulse);
        currentThrowable.GetComponent<Rigidbody2D>().MoveRotation(90);
    }

    #region Teleport
    //Esto te teletransporta hacia el objeto
    private void TeleportToThrowable()
    {
        Vector2 throwablePivot = currentThrowable.GetComponent<SpriteRenderer>().sprite.pivot / currentThrowable.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector2 teleportDestination = currentThrowable.transform.position - new Vector3(0,throwablePivot.y + 0.4f,0);

        Vector2 offset = CheckTeleportCollisions();
        transform.position = teleportDestination + offset;


        Destroy(currentThrowable);
    }

    private Vector2 CheckTeleportCollisions()
    {
        Vector2 adjustedOffset = Vector2.zero;

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, playerHeight / 2, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, playerWidth / 2, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, playerWidth / 2, groundLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, playerHeight / 2, groundLayer);

        adjustedOffset += ProcessCollision(hitUp);
        adjustedOffset += ProcessCollision(hitLeft);
        adjustedOffset += ProcessCollision(hitRight);
        adjustedOffset += ProcessCollision(hitDown);
        return adjustedOffset;
    }

    private Vector2 ProcessCollision(RaycastHit2D hit)
    {
        Vector2 offset = Vector2.zero;

        if (hit.collider != null)
        {
            offset = hit.normal * Mathf.Abs(hit.distance);

            float speed = GetComponent<Rigidbody2D>().velocity.magnitude;
            offset *= speed * Time.deltaTime;
        }

        return offset;
    }

    #endregion

    public void DestroyThrowable()
    {
        if (currentThrowable != null)
        Destroy(currentThrowable);

    }

    //Esto muestra la trajectoria del projectil, he de decir que los calculos los he hecho con internet y chatGpt porque no se mucho de mates
    private void ShowTrajectory()
    {
        //Primero me aseguro que el lineRenderer este enabled
        lineRenderer.enabled = true;

        //Aqui decido el ancho de la linea
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        //Esta variable es para que no se dibuje infinitamente una linea
        float maxDrawTime = 0.9f;
        //Esto calcula la cantidad de posiciones que tiene el lineRenderer para que luego no de ningun error de indexOutOfBounds
        lineRenderer.positionCount = Mathf.CeilToInt((linePoints / timeBetweenPoints) + 1);
        //Esta i es basicamente la posicion de que se esta utilizando actualmente del line renderer
        int i = 1;
        //Aqui creo un vector que sera la trajectoria
        Vector2 trajectory = new Vector2(0, 0);
        //Pongo la primera posicion del lineRenderer como la posicion de donde sale el objeto
        lineRenderer.SetPosition(0, objectSpawnPoint.position);

        //Este float dibuja la linea del lineRenderer
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            if (time > maxDrawTime)
            {
                //Esto no se si es correcto
                break;
            }

            //Aqui calculo la posicion inicial utilizan
            float x = objectSpawnPoint.position.x + time * throwDirection.x * throwForce;
            float y = objectSpawnPoint.position.y + throwDirection.y * throwForce * time + 0.5f * Physics.gravity.y * time * time;
            trajectory = new Vector3(x, y, objectSpawnPoint.position.z);
            lineRenderer.SetPosition(i, trajectory);
            i++;

        }
        for (; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, trajectory);
        }
    }

    //Esto borra la trajectorio del line renderer
    private void HideTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

    //Esto baja la gravedad para que tengas mas tiempo de pensar a donde tirar el objeto
    public void SlowMo()
    {
        rb2D.gravityScale = sloMoGravity;

    }

    //Esto sube a la gravedad a su estado inicial
    public void StopSloMo()
    {
        rb2D.gravityScale = preSloMoGravity;
    }

    private void OnFacingAt(InputAction.CallbackContext value)
    {
        Vector2 previousValue = throwDirection;
        if (value.ReadValue<Vector2>() == Vector2.zero)
        {
            inputDirection = previousValue;
        }
        else
        {
            inputDirection = value.ReadValue<Vector2>();
        }
        if (inputGraceTimer > inputGraceTimerLimit)
        {
            inputDirection = defaultThrow * movement.lookingDirection;
        }

    }


}

