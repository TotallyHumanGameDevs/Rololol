using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    public Image image; 
    public float fadeSpeed;
    bool fading = false;
    float alpha = 0f;

    void Awake(){
        //PlayerPrefs.SetInt("levelReached", 7);
        //PlayerPrefs.SetString("bonusLevelsCompleted", "23456");

        if(instance != null){
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    IEnumerator FadeOut(int num){
        fading = true;
        alpha = 0f;
        while(alpha < 1f){
            alpha += fadeSpeed * Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        SceneManager.LoadScene(num);
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn(){
        alpha = 1f;
        while(alpha > 0f){
            alpha -= fadeSpeed * Time.deltaTime;
            image.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fading = false;
    }

    public void RestartLevel(){
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadLevel(int num){
        if(!fading)
            StartCoroutine(FadeOut(num));
    }

    public int GetLevelNum(){
        return SceneManager.GetActiveScene().buildIndex;
    }
}
