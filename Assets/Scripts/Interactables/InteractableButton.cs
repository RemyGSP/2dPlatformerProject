using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private Sprite pressedButton;
    //Aqui se pone el objeto que sea el obstaculo que se eliminara cuando se pulse el boton
    [SerializeField] private GameObject buttonObstacle;
    [SerializeField] private AudioClip buttonSFX;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Beacon"))
        {
            Destroy(collision.gameObject);
        }
        this.GetComponent<SpriteRenderer>().sprite = pressedButton;
        buttonObstacle.GetComponent<LaserRayObstacleAnimation>().DisapearObstacle();
        this.GetComponent<BoxCollider2D>().enabled = false;
        AudioManager.Instance.sfxSource.PlayOneShot(buttonSFX);
    }
}
