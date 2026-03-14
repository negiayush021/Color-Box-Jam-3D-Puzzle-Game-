using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public GameObject LevelsMenu;
    public GameObject MainMenu;

    public TextMeshProUGUI coins_text;

    public TextMeshProUGUI heart_timer_txt;
    public TextMeshProUGUI heartNum_txt;

    [SerializeField] Image fadePanelimg;
    [SerializeField] GameObject fadePanel;

    public static DataManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("CurrentHearts"))
        {
            PlayerPrefs.SetInt("CurrentHearts", 5);
        }

        fadePanel.SetActive(false);
        Color c = fadePanelimg.color;
        c.a = 0;
        fadePanelimg.color = c;
        Levels_manage();
        Application.targetFrameRate = 60;
        LoadData();
    }
    private void Update()
    {
        UpdateUI();
    }
    public void Play_btn()
    {
        LevelsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void Exit_btn()
    {
        Application.Quit();
    }

    

    int coins;

    int currentHearts = 5;
    int maxHearts = 5;
    int refillTimeSeconds = 1800;
    private DateTime lastHeartLostTime;

    void UpdateUI()
    {
        coins_text.text = coins.ToString();
        heartNum_txt.text = currentHearts.ToString();

        if (currentHearts < maxHearts)
        {
            TimeSpan timeSinceLastLoss = DateTime.Now - lastHeartLostTime;
            int totalSeconds = (int)timeSinceLastLoss.TotalSeconds;

            if (totalSeconds >= refillTimeSeconds)
            {
                int heartsToAdd = totalSeconds / refillTimeSeconds;

                currentHearts += heartsToAdd;
                if (currentHearts > maxHearts) currentHearts = maxHearts;

                lastHeartLostTime = DateTime.Now;
                SaveData();
            }
            else
            {
                int remainingSeconds = refillTimeSeconds - totalSeconds;
                
                heart_timer_txt.text = $"{remainingSeconds / 60:D2}:{remainingSeconds % 60:D2}";


            }
        }
        else
        {
            heart_timer_txt.text = "Full";
           
        }

    }

    void LoadData()
    {
        coins = PlayerPrefs.GetInt("coins");

        currentHearts = PlayerPrefs.GetInt("CurrentHearts", currentHearts);
        string timeString = PlayerPrefs.GetString("LastHeartLostTime", DateTime.Now.ToString());
        DateTime.TryParse(timeString, out lastHeartLostTime);
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("CurrentHearts", currentHearts);
        PlayerPrefs.SetString("LastHeartLostTime", lastHeartLostTime.ToString());
        PlayerPrefs.Save();
    }



    public void Level_1()
    {
        LoadScene(2);
    }
    public void Level_2()
    {
        LoadScene(3);
    }
    public void Level_3()
    {
        LoadScene(4);
    }
    public void Level_4()
    {
        LoadScene(5);
    }
    public void Level_5()
    {
        LoadScene(6);
    }
    public void Level_6()
    {
        LoadScene(7);
    }
    public void Level_7()
    {
        LoadScene(8);
    }
    public void Level_8()
    {
        LoadScene(9);
    }
    public void Level_9()
    {
        LoadScene(10);
    }
    public void Level_10()
    {
        LoadScene(11);
    }
    public void Level_11()
    {
        LoadScene(12);
    }
    public void Level_12()
    {
        LoadScene(13);
    }
    public void Level_13()
    {
        LoadScene(14);
    }
    public void Level_14()
    {
        LoadScene(15);
    }
    public void Level_15()
    {
        LoadScene(16);
    }
    public void Level_16()
    {
        LoadScene(17);
    }
    public void Level_17()
    {
        LoadScene(18);
    }

    void LoadScene(int id)
    {
        fadePanel.SetActive(true);
        StartCoroutine(fadeAndLoad(id));
    }

    IEnumerator fadeAndLoad(int id)
    {
        // Fade from 0 to 1 alpha
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Color c = fadePanelimg.color;
            c.a = t;
            fadePanelimg.color = c;
            yield return null;

        }
        SceneManager.LoadScene(id);
    }
    

    public GameObject[] locked_img;
    void Levels_manage()
    {
        for (int i = 0; i < locked_img.Length; i++)
        {
            if (PlayerPrefs.GetInt("Level cleared") >= i)
            {
                locked_img[i].SetActive(false);
            }
        }

        
    }
}
