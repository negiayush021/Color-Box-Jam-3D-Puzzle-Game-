using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class CoinsManager : MonoBehaviour
{
    [Header("victory screen")]
    public int coins;
    public TextMeshProUGUI coins_text;

    public int will_get_coins;
    public TextMeshProUGUI coins_get_txt;

    [Header("refill your lives screen")]
    public TextMeshProUGUI coins_text2;

    [Header("buy power ups")]
    public TextMeshProUGUI coins_text3;
    public TextMeshProUGUI coins_text4;
    public TextMeshProUGUI coins_text5;


    public static CoinsManager instance;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        loadData();
    }

    private void Start()
    {
        

        print("coins :"+PlayerPrefs.GetInt("coins"));

        will_get_coins = 30;
        coins_get_txt.text = "+" + will_get_coins.ToString();
    }


    void Update()
    {
        UpdateUI();
        
    }

    public void saveCoins()
    {
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.Save();
    }

    public void loadData()
    {
        coins = PlayerPrefs.GetInt("coins");
    }

    public void UpdateUI()
    {
        coins_text.text = coins.ToString();
        coins_text2.text = coins.ToString();
        if (GameManager.Instance.current_level >= 4)
        {
            coins_text3.text = coins.ToString();
        }

        if (GameManager.Instance.current_level >= 9)
        {
            coins_text4.text = coins.ToString();
        }

        if (GameManager.Instance.current_level >= 14)
        {
            coins_text5.text = coins.ToString();
        }

    }

    public void AddCoins(int amount)
    {
        coins += amount;
        saveCoins();
    }

    [Space(200)]
    [SerializeField] GameObject notEnoughConins_image;

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            
            coins -= amount;
            saveCoins();

            return true;

        }
        else
        {
            GameObject temp=  Instantiate(notEnoughConins_image, CanvasManager.instance.transform);
            Destroy(temp, 2f);
            MusicManager.instance.PlayClip(15);
            print("Not enough coins!");
            return false;
        }
    }

    [SerializeField] Button tripleRewardBtn;
    public void get_triple_reward()
    {
        Ads.Instance.ads_for_coins = true;
        Ads.Instance.ShowAd();

        tripleRewardBtn.gameObject.SetActive(false);
    }

}
