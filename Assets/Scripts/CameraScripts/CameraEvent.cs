using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraEvent : MonoBehaviour
{
    public UnityEvent<GameObject> changeCamera;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        changeCamera.Invoke(this.gameObject);
    }
}
