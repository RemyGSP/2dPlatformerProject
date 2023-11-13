using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Falta hacer que haya un volumen para la musica y para los sfx, en un futuro, y de momento el volumen solo se puede cambiar desde el script porque options no funciona
    static public float volume = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AudioSource>().volume = volume;
    }


}
