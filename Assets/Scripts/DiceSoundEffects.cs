using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSoundEffects : MonoBehaviour
{
    public AudioClip[] woodClip, grassClip, metalClip;

    AudioSource audioSource;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGrass(){
        audioSource.clip = grassClip[Random.Range(0, grassClip.Length)];
        audioSource.Play();
    }

    public void PlayWood(){
        audioSource.clip = woodClip[Random.Range(0, woodClip.Length)];
        audioSource.Play();
    }

    public void PlayMetal(){
        audioSource.clip = metalClip[Random.Range(0, metalClip.Length)];
        audioSource.Play();
    }
}
