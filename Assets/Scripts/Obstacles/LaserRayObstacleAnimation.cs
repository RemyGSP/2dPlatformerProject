using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRayObstacleAnimation : MonoBehaviour
{

    public float shrinkDuration = 1.5f; 
    public float finalWidth = 0.1f; 

    public void DisapearObstacle()
    {
        StartCoroutine(ShrinkOverTime(shrinkDuration));
    }
    IEnumerator ShrinkOverTime(float duration)
    {
        float timer = 0f;
        Vector3 initialPosition = transform.position;
        float initialWidth = transform.localScale.x;

        while (timer < duration)
        {
            float progress = timer / duration;
            float newWidth = Mathf.Lerp(initialWidth, finalWidth, progress);
            float newPositionX = initialPosition.x + (initialWidth - newWidth) / 2f; 
            transform.localScale = new Vector3(newWidth, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
            timer += Time.deltaTime;
            yield return null;
        }

        float finalPositionX = initialPosition.x + (initialWidth - finalWidth) / 2f;
        transform.localScale = new Vector3(finalWidth, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(finalPositionX, transform.position.y, transform.position.z);
        gameObject.SetActive(false);
    }
}

