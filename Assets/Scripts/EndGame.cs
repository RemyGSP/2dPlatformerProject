using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public UnityEvent End;
    // TODO ESTE SCRIPT ES TEMPORAL DE MOMENTO NO SE COMO HARE LA CONDICION DE VICTORIA ESTO ES SOLO PARA EL MOMENTO

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if  (collision.gameObject.CompareTag("Player"))
        Finish();
    }
    public void Finish()
    {
        //Aqui los objetos que se tengan que desactivar de momento hare un evento luego ya vere si tengo que ampliar el codigo
        End.Invoke();
    }

    public void Reset()
    {
        //Reseteo la scene, un poco cutre
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
