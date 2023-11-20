using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMusicSlider : MonoBehaviour
{
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = AudioOptionsManager.musicVolume;
    }

}
