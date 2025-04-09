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

    void Start()
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
