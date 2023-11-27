using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip tpSound;

    public void PlayJumpSound()
    {
        AudioManager.Instance.sfxSource.PlayOneShot(jumpSound);
    }
    public void PlayFootstepSound()
    {
        StartCoroutine(Steps());
        AudioManager.Instance.sfxSource.PlayOneShot(footstepSound);
    }
    //Hacer esto para que haya un timing entre los sonidos de los pasos
    IEnumerator Steps()
    {
        yield return new WaitForSeconds(0.1f);
    }
    public void PlayThrowSound()
    {
        AudioManager.Instance.sfxSource.PlayOneShot(throwSound);
    }

    public void PlayTpSound()
    {
        AudioManager.Instance.sfxSource.PlayOneShot(tpSound);
    }
}