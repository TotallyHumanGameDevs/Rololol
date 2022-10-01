using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;


public class LevelSelector : MonoBehaviour
{
    public Transform buttons;
    /*public Button[] beginnerLevelButtons;
    public GameObject[] beginnerCovers;
    public GameObject[] beginnerChecks;
    public Button[] hardLevelButtons;
    public GameObject[] hardCovers;
    public GameObject[] hardChecks;*/
    public TMP_Text levelsCompleteText;
    public GameObject musicEnabledCheck;


    void Start(){
        if(PlayerPrefs.GetInt("Music Enabled", 1) == 0)
            musicEnabledCheck.SetActive(false);

        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        string bonusLevelsCompleted = PlayerPrefs.GetString("bonusLevelsCompleted", "");
        List<int> sepBonusLvls = new List<int>();
        foreach(char ch in PlayerPrefs.GetString("bonusLevelsCompleted")){
            sepBonusLvls.Add(ch - '0');
        }

        for (int i = 0; i < 6; i++)
        {
            if(i < levelReached - 1)
                buttons.GetChild(i).GetChild(1).gameObject.SetActive(true);
            else if (i >= levelReached){
                buttons.GetChild(i).GetComponent<Button>().interactable = false;
                buttons.GetChild(i).GetChild(2).gameObject.SetActive(true);
            }
        }
        if(levelReached <= 6){
            for (int i = 0; i < 6; i++)
            {
                buttons.GetChild(i + 6).GetComponent<Button>().interactable = false;
                buttons.GetChild(i + 6).GetChild(2).gameObject.SetActive(true);
            }
        }
        else{
            foreach(int n in sepBonusLvls){
                buttons.GetChild(n + 5).GetChild(1).gameObject.SetActive(true);
            }
        }

        int levelsComplete = PlayerPrefs.GetInt("levelReached") + sepBonusLvls.Count - 1;

        if(levelsComplete < 12)
            buttons.GetChild(12).gameObject.SetActive(false);

        if(levelsComplete < 13)
            levelsCompleteText.text = "Levels Complete: " + (levelsComplete != -1 ? levelsComplete : 0);
        else
            levelsCompleteText.text = "Thanks for playing!";
    }


    public void Select(int num){
        SceneManagement.instance.LoadLevel(num);
    }

    public void ToggleMusic(){
        int enabled = PlayerPrefs.GetInt("Music Enabled");
        PlayerPrefs.SetInt("Music Enabled", enabled == 1 ? 0 : 1);
        if(enabled == 0){
            MusicManager.instance.PlayMusic();
            musicEnabledCheck.SetActive(true);
        }
        else{
            MusicManager.instance.StopMusic();
            musicEnabledCheck.SetActive(false);
        }
    }
}
