using UnityEngine;
using TMPro;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }



    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
                AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }
}