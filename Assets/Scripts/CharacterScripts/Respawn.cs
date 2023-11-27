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
    [SerializeField] private GameObject cameraManager;
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
            ChangeCameraToRespawnCamera();
            onRespawn.Invoke();
        }
    }

    public void ChangeRespawn(GameObject newRespawn)
    {
        currentSpawn = newRespawn;
    }

    public void ChangeCameraToRespawnCamera()
    {
        cameraManager.GetComponent<ChangeCamera>().OnCameraChange(currentSpawn.GetComponent<ChangeRespawnPoint>().respawnPointCamera);
    }
}
