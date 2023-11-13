using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    private Slider musicSlider;

    private void Start()
    {
        musicSlider = GetComponent<Slider>();
        musicSlider.value = AudioManager.volume;
    }
}
