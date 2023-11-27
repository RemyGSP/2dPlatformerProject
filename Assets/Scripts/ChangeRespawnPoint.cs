using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRespawnPoint : MonoBehaviour
{
    [SerializeField] private Respawn respawnManager;
    public GameObject respawnPointCamera;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        respawnManager.ChangeRespawn(this.gameObject);
    }


}
