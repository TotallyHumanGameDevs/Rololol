using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    void Awake(){
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume", 0.66f);
    }

    public void ChangeValue(){
        PlayerPrefs.SetFloat("Volume", GetComponent<Slider>().value);
        AudioListener.volume = GetComponent<Slider>().value + 0.4f;
    }
}
