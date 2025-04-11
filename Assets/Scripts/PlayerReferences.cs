using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    public static PlayerReferences instance;
    [SerializeField]
    private Rigidbody2D rigidbody;
    [SerializeField]
    private GameObject visuals;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerCollisions playerCollisions;
    private int playerLookingDirection;
    [SerializeField]
    private FlipSprite characterSpriteFlip;
    [SerializeField]
    SpriteRenderer characterRenderer;
    [SerializeField]
    private InputReceiver inputReceiver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            print("A playerReferences instance already exists");
        }
    }
    void Start()
    {

    }

    public PlayerCollisions GetPlayerCollisions()
    {
        return playerCollisions;
    }
    public Animator GetAnimator()
    {
        return animator;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rigidbody;
    }

    public GameObject GetVisuals()
    {
        return visuals;
    }

    public void SetPlayerLookingDirection(int lookingDirection)
    {
        playerLookingDirection = lookingDirection;
    }

    public FlipSprite GetSpriteFlipper()
    {
        return characterSpriteFlip;
    }
    public int GetPlayerLookingDirection()
    {
        return playerLookingDirection;
    }

    public InputReceiver GetInput()
    {
        return inputReceiver;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return characterRenderer;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
