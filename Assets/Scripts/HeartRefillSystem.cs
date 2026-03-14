using UnityEngine;
using TMPro;
using System;

public class HeartRefillSystem : MonoBehaviour
{
    [Header("Heart Settings")]
    public int maxHearts = 5;
    public int currentHearts;
    public int refillTimeSeconds = 1800; // 30 minutes in seconds

    [Header("UI References")]
    public TextMeshProUGUI heartsText;
    public TextMeshProUGUI heartsText2;
    public TextMeshProUGUI heartsText3;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerText2;
    public TextMeshProUGUI timerText3;
    public TextMeshProUGUI timerText4;


    //this is for OutOfSpace window
    public TextMeshProUGUI heartsText4;
    public TextMeshProUGUI timerText5;


    public GameObject refillPage; // Assign the "Refill Your Heart" page in Inspector

    private DateTime lastHeartLostTime;


    public static HeartRefillSystem instance;
    private void Awake()
    {
        instance = this;
    }

    //[SerializeField] GameObject Heart_refill_crossbtn;
    void Start()
    {
       
        LoadData();
        if (currentHearts <= 0)
        {
            
            //Heart_refill_crossbtn.SetActive(false);
            refillPage.SetActive(true);
        }
    }

    void Update()
    {
        
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
                timerText.text = $"{remainingSeconds / 60:D2}:{remainingSeconds % 60:D2}";
                timerText2.text = $"{remainingSeconds / 60:D2}:{remainingSeconds % 60:D2}";
                timerText3.text = $"{remainingSeconds / 60:D2}:{remainingSeconds % 60:D2}";
                timerText4.text = $"{remainingSeconds / 60:D2}:{remainingSeconds % 60:D2}";
                timerText5.text = $"{remainingSeconds / 60:D2}:{remainingSeconds % 60:D2}";
                

            }
        }
        else
        {
            timerText.text = "Full";
            timerText2.text = "Full";
            timerText4.text = "Full";
            timerText5.text = "Full";

             
           
        }

        UpdateUI();
    }

    public void LoseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            if (currentHearts < maxHearts)
            {
                lastHeartLostTime = DateTime.Now;
            }

            SaveData();
            UpdateUI();
            GameManager.Instance.retry();
           
        }
        else
        {
            print("current hearts : " + currentHearts);
            MusicManager.instance.PlayClip(5);
            refillPage.SetActive(true); // Show refill page
        }
    }

    void UpdateUI()
    {
        heartsText.text = currentHearts.ToString();
        heartsText2.text = currentHearts.ToString();
        heartsText3.text = currentHearts.ToString();
        heartsText4.text = currentHearts.ToString();
       
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("CurrentHearts", currentHearts);
        PlayerPrefs.SetString("LastHeartLostTime", lastHeartLostTime.ToString());
        PlayerPrefs.Save();
    }

    void LoadData()
    {
        currentHearts = PlayerPrefs.GetInt("CurrentHearts", currentHearts);
        string timeString = PlayerPrefs.GetString("LastHeartLostTime", DateTime.Now.ToString());
        DateTime.TryParse(timeString, out lastHeartLostTime);
    }

    [SerializeField] GameObject heartFullScreen;
    public GameObject EarnAheart_screen;
    public void buyHearts()
    {
        
       if (CoinsManager.instance.SpendCoins(500))
        {
            
            currentHearts = 5;
            SaveData();
            UpdateUI();
            CoinsManager.instance.SpendCoins(500);
            MusicManager.instance.PlayClip(8);
            heartFullScreen.SetActive(true);
        }
        
    }

    //when player buy hearts with coin then this button appear
    public void closeRefillpage()
    {
        MusicManager.instance.PlayClip(6);
        refillPage.SetActive(false);
    }

    public void EarnAheart()
    {
        Ads.Instance.ads_for_hearts = true;
        Ads.Instance.ShowAd();
    }
    
}
