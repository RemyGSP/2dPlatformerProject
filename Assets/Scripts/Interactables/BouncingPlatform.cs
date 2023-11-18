using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncingPlatºorm : MonoBehaviour
{
    [Header("Bounce Values")]

    [SerializeField] private Vector2 maxBounceSpeed;
    [SerializeField] Vector2 bounceMultiplier;
    [SerializeField] private GameObject platformParticles;

    private void Start()
    {
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        GameObject particles = Instantiate(platformParticles);
        particles.transform.position = collision.GetContact(0).point;
        float xSpeed = rb.velocity.x > maxBounceSpeed.x ? maxBounceSpeed.x : rb.velocity.x;
        float ySpeed = rb.velocity.y > maxBounceSpeed.y ? maxBounceSpeed.y : rb.velocity.y;
        rb.velocity = new Vector2(xSpeed,ySpeed) * bounceMultiplier; 
    }


}