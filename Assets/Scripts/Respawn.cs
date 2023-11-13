using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform firstSpawn;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private GameObject player;
    public UnityEvent onRespawn; 
    public void RespawnPlayer()
    {
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = player.transform.position;
            foreach (Transform t in respawnPoints)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            player.transform.position = tMin.position;

            //Esto es para poder borrar el beacon en el caso de que haya uno cuando mueras
            onRespawn.Invoke();
        }
    }
}
