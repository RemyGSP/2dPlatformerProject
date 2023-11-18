using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioManager;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioClip throwSound;
    public void PlayJumpSound()
    {
        audioManager.PlayOneShot(jumpSound);
    }
    //public void PlayFootstepSound()
    //{
    //}
    //Hacer esto para que haya un timing entre los sonidos de los pasos
    IEnumerator Steps()
    {
        yield return new WaitForSeconds(0.1f);
    }
    public void PlayThrowSound()
    {
        audioManager.PlayOneShot(throwSound);
    }
}