using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillCharacter : MonoBehaviour
{
    public UnityEvent playerDead;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Aqui implementar codigo para la animacion de muerte del persona o yo que se
        if (collision.gameObject.CompareTag("Player"))
            playerDead.Invoke();
        else Destroy(collision.gameObject);
    }
}
