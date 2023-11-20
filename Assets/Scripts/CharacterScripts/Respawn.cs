using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject firstSpawn;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathEffects;
    private GameObject currentSpawn;
    public UnityEvent onRespawn;

    private void Start()
    {
        currentSpawn = firstSpawn;
    }
    public void RespawnPlayer()
    {
        {
            GameObject a = Instantiate(deathEffects);
            a.transform.position = player.transform.position;
            player.transform.position = currentSpawn.transform.position;

            //Transform tMin = null;
            //float minDist = Mathf.Infinity;
            //Vector3 currentPos = player.transform.position;
            //foreach (Transform t in respawnPoints)
            //{
            //    float dist = Vector3.Distance(t.position, currentPos);
            //    if (dist < minDist)
            //    {
            //        tMin = t;
            //        minDist = dist;
            //    }
            //}

            //Esto es para poder borrar el beacon en el caso de que haya uno cuando mueras
            onRespawn.Invoke();
        }
    }

    public void ChangeRespawn(GameObject newRespawn)
    {
        currentSpawn = newRespawn;
    }
}
