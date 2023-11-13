using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitMechanic : MonoBehaviour
{
    //Indice 0 izquierda, 1 derecha, 2 arriba, 3 abajo, 4 arriba izquierda, 5 arriba derecha, 6 abajo izquierda, 7 abajo derecha
    [Header("Hit Position")]
    [SerializeField] GameObject left, right, up, down, upLeft, upRight, downLeft, downRight;
    private Movement movement;
    [SerializeField] private Vector3 hitDirection;
    [SerializeField] private Vector3 hitCurrentPosition;
    void Start()
    {
        movement = GetComponent<Movement>();
    }



    public void OnFire(InputValue value)
    {

            if (GetClosestPosition() != null)
            {
                Debug.Log(GetClosestPosition());
            }
        
    }

    private GameObject GetClosestPosition()
    {
        GameObject closestPosition = null;

        hitDirection = new Vector3(movement.playerDirection.x, movement.verticalMove.y,0);
        List<GameObject> positions = new List<GameObject>();
        positions.Add(left); positions.Add(right); positions.Add(up); positions.Add(down); positions.Add(upLeft); positions.Add(upRight); positions.Add(downLeft); positions.Add(downRight);
        float closestDistance = Mathf.Infinity;

        foreach (GameObject hitPosition in positions)
        {
            if (hitPosition != null)
            {
                float distance = Vector3.Distance(transform.position + hitDirection, hitPosition.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = hitPosition;
                }
            }
        }
        return closestPosition;
    }
}
