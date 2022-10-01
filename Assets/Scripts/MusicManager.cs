using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;


    void Awake(){
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.66f) + 0.4f;
        if(PlayerPrefs.GetInt("Music Enabled", 1) == 0){
            StopMusic();
        }

        if(instance != null){
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic() => GetComponent<AudioSource>().Play();
    public void StopMusic() => GetComponent<AudioSource>().Stop();
}
