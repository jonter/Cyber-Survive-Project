using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSettings : MonoBehaviour
{
    [SerializeField] Slider generalSlider;
    [SerializeField] Slider musicSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetupSliders();
        generalSlider.onValueChanged.AddListener(OnGeneralPressed);
        musicSlider.onValueChanged.AddListener(OnMusicPressed);
    }

    void SetupSliders()
    {
        if (PlayerPrefs.HasKey("general-volume") == false)
            PlayerPrefs.SetFloat("general-volume", 1);
        if (PlayerPrefs.HasKey("music-volume") == false)
            PlayerPrefs.SetFloat("music-volume", 1);

        float genVolume = PlayerPrefs.GetFloat("general-volume");
        float musicVolume = PlayerPrefs.GetFloat("music-volume");

        generalSlider.value = genVolume; 
        musicSlider.value = musicVolume;
    }

    void OnGeneralPressed(float value)
    {
        PlayerPrefs.SetFloat("general-volume", value);
        AudioListener.volume = value;
    }

    void OnMusicPressed(float value)
    {
        PlayerPrefs.SetFloat("music-volume", value);
        MusicManager.instance.SetMaxVolume(value);
    }





}
