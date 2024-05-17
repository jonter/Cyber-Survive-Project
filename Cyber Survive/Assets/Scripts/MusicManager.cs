using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    AudioSource audio;
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip battleMusic;

    float maxVolume = 1;
    
    public void SetMaxVolume(float max)
    {
        maxVolume = max;
        audio.DOKill();
        audio.volume = maxVolume;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            audio = GetComponent<AudioSource>();
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (PlayerPrefs.HasKey("music-volume") == true)
                maxVolume = PlayerPrefs.GetFloat("music-volume");
        }
        else Destroy(gameObject);
    }

    public void SetMenuMusic()
    {
        StartCoroutine(SwitchMusic(menuMusic));
    }

    public void SetBattleMusic()
    {
        StartCoroutine(SwitchMusic(battleMusic));
    }

    IEnumerator SwitchMusic(AudioClip newClip)
    {
        audio.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        audio.clip = newClip;
        audio.Play();
        audio.DOFade(maxVolume, 1);
    }


}
