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

        //Aqui hacer evento de muerte del jugador ESTO SE PODRIA CAMBIAR DE LUGAR PERFECTAMENTE DE HECHO SE TENDRIA QUE HACER EN EL CASO DE QUE SE AÑADA OTRA FORMA DE MATAR AL PERSONAJE O NO, DA IGUAL
        playerDead.Invoke();
    }
}
