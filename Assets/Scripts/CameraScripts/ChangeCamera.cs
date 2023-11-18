using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    static GameObject activeCamera;
    [SerializeField] private CinemachineVirtualCamera dialogueCamera;
    [SerializeField] private GameObject[] cameraList;
    void Start()
    {
        activeCamera = cameraList[0];
    }


    public void OnCameraChange(GameObject newCamera)
    {
        if (activeCamera != this.gameObject)
        {
            activeCamera.GetComponent<CinemachineVirtualCamera>().Priority = 10;
            activeCamera = newCamera;
            activeCamera.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        }
    }

}
