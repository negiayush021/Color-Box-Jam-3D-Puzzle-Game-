using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
   public static CanvasManager instance;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    [Header("Freeze Power details")]

    public  GameObject freezeTuto;
    public int freezePow_num;
    public TextMeshProUGUI freezePow_num_txt;
    [SerializeField] GameObject _greenCirclePlus1;
    [SerializeField] Image triangle1;

    public GameObject slider;
    public Slider freeze_slider;
    public bool freezeCountDown = false;
    public GameObject freezing_img;
    public Image freezing_img_image;

    [Header("Fill Queue Power deatils")]

    public GameObject FillQueueTuto;
    public int FillQueuePow_num;
    public TextMeshProUGUI FillQueuePow_num_txt;
    [SerializeField] GameObject _greenCirlePlus2;
    [SerializeField] Image triangle2;


    public bool fillQueuePow_AlreadyinUse = false;

    [Header("Destroy Obstacle Power details")]

    [SerializeField] Image triangle3;
    public GameObject DestroyObstacleTuto;
    public int DestroyObstaclePow_num;
    public TextMeshProUGUI DestroyObstaclePow_num_txt;
    [SerializeField] GameObject _greencirclePlus3;

    public GameObject PowerUseTxt_Object;
    public bool IsThirdPowerUse;

    public GameObject Hammer_prefab;
    public GameObject smoke_effect;


    public void LoadPow()
    {
        freezePow_num = PlayerPrefs.GetInt("FreezePow" , freezePow_num);
        FillQueuePow_num = PlayerPrefs.GetInt("FillQueuePow" , FillQueuePow_num);
        DestroyObstaclePow_num = PlayerPrefs.GetInt("DestroyObstaclePow" , DestroyObstaclePow_num);
    }

    public void SavePowfreeze()
    {
        

        // FreezePower code
        if (freezePow_num <= 0)
        {
            _greenCirclePlus1.SetActive(true);
            freezePow_num = 0;
        }
        else
        {
            _greenCirclePlus1.SetActive(false);
        }
        PlayerPrefs.SetInt("FreezePow", freezePow_num);
        freezePow_num_txt.text = freezePow_num.ToString();



        PlayerPrefs.Save();
    }

    void SavePowfillQueue()
    {
        //FillQueuePower code
        if (FillQueuePow_num <= 0)
        {
            _greenCirlePlus2.SetActive(true);
            FillQueuePow_num = 0;
        }
        else
        {
            _greenCirlePlus2.SetActive(false);
        }
        PlayerPrefs.SetInt("FillQueuePow", FillQueuePow_num);
        FillQueuePow_num_txt.text = FillQueuePow_num.ToString();



        PlayerPrefs.Save();
    }

    void SavePowDestroyObstacle()
    {
        if(DestroyObstaclePow_num <= 0)
        {
            _greencirclePlus3.SetActive(true);
            DestroyObstaclePow_num = 0;
        }
        else
        {
            _greencirclePlus3.SetActive(false);
        }
        PlayerPrefs.SetInt("DestroyObstaclePow", DestroyObstaclePow_num);
        DestroyObstaclePow_num_txt.text = DestroyObstaclePow_num.ToString();

        PlayerPrefs.Save();
    }

    bool onetime_onvibrate = true;
    bool onetime_offvibrate = true;
    bool onetime_onsound = true;
    bool onetime_offsound = true;
    bool onetime_onmusic = true;
    bool onetime_offmusic = true;

    [Space(50)]

    public GameObject RunOutOfTime_screen;
    public GameObject OutOfSpace_screen;
    [Header("Additional")]

    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider vibrateSlider;


    private void Start()
    {
        if (GameManager.Instance.current_level > 3)
        {
            LoadPow();
        }
        

        if (PlayerPrefs.GetInt("Music")== 0)
        {
            onMusic();
        }
        else if (PlayerPrefs.GetInt("Music")== 1)
        {
            offMusic();
        }


        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            onSound();
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            offSound();
        }


        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            onVibrate();
        }
        else if (PlayerPrefs.GetInt("Vibrate") == 1)
        {
            offVibrate();
        }
    }
    private void Update()
    {
        if (GameManager.Instance.current_level >= 4)
        {
            SavePowfreeze();
        }

        if (GameManager.Instance.current_level >= 9)
        {
            SavePowfillQueue();
        }
        if (GameManager.Instance.current_level >= 14)
        {
            SavePowDestroyObstacle();
        }
        
    }

    [SerializeField] private GameObject settings_page;

    public void OpenSettings()
    {
        MusicManager.instance.PlayClip(5);
        settings_page.SetActive(true);
        Timer.Instance.counting_down = false;
    }

    public void CloseSettings()
    {
        MusicManager.instance.PlayClip(6);
        settings_page.SetActive(false);
        Timer.Instance.counting_down = true;
    }

    public GameObject offMusicButton;
    public void offMusic()
    {
        
            MusicManager.instance.audioSource_music.mute = true;
        if (!onetime_offmusic)
        {
            MusicManager.instance.PlayClip(12);
        }
            onetime_offmusic = false;
            offMusicButton.SetActive(true);
            PlayerPrefs.SetInt("Music", 1);
        musicSlider.value = 0;
        

    }
    public void onMusic()
    {
                    MusicManager.instance.audioSource_music.mute = false;
        if (!onetime_onmusic)
        {
            MusicManager.instance.PlayClip(12);
        }
            onetime_onmusic = false;
            offMusicButton.SetActive(false);
            PlayerPrefs.SetInt("Music", 0);
        musicSlider.value = 1;
        
    }


    public GameObject offSoundButton;
    public void offSound()
    {
        MusicManager.instance.can_play_sound = false;
        if (!onetime_offsound)
        {
            MusicManager.instance.PlayClip(12);
        }
        onetime_offsound = false;
        offSoundButton.SetActive(true);
        PlayerPrefs.SetInt("Sound", 1);
        soundSlider.value = 0;
        
    }
    public void onSound()
    {
        MusicManager.instance.can_play_sound = true;
        if (!onetime_onsound)
        {
            MusicManager.instance.PlayClip(12);
        }
        onetime_onsound = false;
        offSoundButton.SetActive(false);
        PlayerPrefs.SetInt("Sound", 0);
        soundSlider.value = 1;
    }

    public bool canVibrate = true;
    public GameObject offVibrateButton;
    public void offVibrate()
    {
        canVibrate = false;
        if (!onetime_offvibrate)
        {
            MusicManager.instance.PlayClip(12);
        }
        onetime_offvibrate = false;
        offVibrateButton.SetActive(true);
        PlayerPrefs.SetInt("Vibrate", 1);
        vibrateSlider.value = 0;
    }
    public void onVibrate()
    {
        canVibrate = true;
        offVibrateButton.SetActive(false);
        if (!onetime_onvibrate)
        {
            MusicManager.instance.PlayClip(12);
        }
        onetime_onvibrate = false;
        PlayerPrefs.SetInt("Vibrate", 0);
        vibrateSlider.value = 1;
    }

    
    

    public void Freezebtn()
    {
       // MusicManager.instance.PlayClip(1);
        if (freezeCountDown== false)
        {
            if (freezePow_num > 0)
            {
                freezePow_num--;
                slider.SetActive(true);
                freezing_img.SetActive(true);
                StartCoroutine(Freezing());
            }
            else
            {
                MusicManager.instance.PlayClip(5);
                buyfreezePowscreen.SetActive(true);
                print("Not enough freeze power");
            }
            
            
        }
        else
        {
            print("Power already in use");
        }
        

    }
    


   

    public void OFFfreezeTutobtn()
    {
        StartCoroutine(offfreezetuto());
        triangle1.color = Color.white;
        MusicManager.instance.PlayClip(14);
    }

    IEnumerator offfreezetuto()
    {
       
        
        yield return new WaitForSeconds(.1f);
        Color c;
        ColorUtility.TryParseHtmlString("#FFD700", out c);
        triangle1.color = c;
        yield return new WaitForSeconds(.3f);
        freezeTuto.SetActive(false);
        PlayerPrefs.SetInt("FreezePowtuto", 1);
    }

    public void OFFfillQueueTutobtn()
    {
        StartCoroutine(offfillQueuetuto());
        triangle2.color = Color.white;
        MusicManager.instance.PlayClip(14);
    }

    IEnumerator offfillQueuetuto()
    {


        yield return new WaitForSeconds(.1f);
        Color c;
        ColorUtility.TryParseHtmlString("#FFD700", out c);
        triangle2.color = c;
        yield return new WaitForSeconds(.3f);
        FillQueueTuto.SetActive(false);
        PlayerPrefs.SetInt("FillQueuePowTuto", 1);
    }

    public void OFFdestroyObstacleTutobtn()
    {
        StartCoroutine(offdestroyObstacletuto());
        triangle3.color = Color.white;
        MusicManager.instance.PlayClip(14);
    }

    IEnumerator offdestroyObstacletuto()
    {
        yield return new WaitForSeconds(.1f);
        Color c;
        ColorUtility.TryParseHtmlString("#FFD700", out c);
        triangle3.color = c;
        yield return new WaitForSeconds(.3f);
        DestroyObstacleTuto.SetActive(false);
        PlayerPrefs.SetInt("DestroyObstaclePowTuto", 1);
    }







    IEnumerator Freezing()
    {
        Timer.Instance.counting_down = false;
        MusicManager.instance.PlayClip(13);
        //timer_icon_freeze.SetActive(true);
        // Fade from 0 to 1 alpha
        for (float t = 0; t < 1; t += Time.deltaTime * 2)
        {
            Color c = freezing_img_image.color;
            c.a = t;
            freezing_img_image.color = c;
            yield return null;


        }

        freezeCountDown = true;
        
        
       
        while (freeze_slider.value != 1)
        {
            
            freeze_slider.value += .15f * Time.deltaTime;
            yield return null;
        }


       
        slider.SetActive(false);
        Timer.Instance.counting_down = true;
        //timer_icon_freeze.SetActive(false);
        freezing_img.SetActive(false);
        freeze_slider.value = 0;
        freezeCountDown = false;
    }

    [SerializeField] GameObject buyfreezePowscreen;
    [SerializeField] GameObject buyfillQueuePowScreen;
    [SerializeField] GameObject buyDestroyObstaclePowScreen;
    [SerializeField] GameObject added_img;
    
    public void closefreezePowScreen()
    {
        MusicManager.instance.PlayClip(6);
        buyfreezePowscreen.SetActive(false);
    }
    public void AddfreezePow()
    {
        
        if (CoinsManager.instance.SpendCoins(120))
        {
            MusicManager.instance.PlayClip(16);
            freezePow_num++;
            GameObject temp = Instantiate(added_img, CanvasManager.instance.transform);
            Destroy(temp, 2f);
        }
        
    }

    
    public void AddFillQueuePow()
    {
        if (CoinsManager.instance.SpendCoins(200))
        {
            FillQueuePow_num++;
            MusicManager.instance.PlayClip(16);
            GameObject temp = Instantiate(added_img, CanvasManager.instance.transform);
            Destroy(temp, 2f);
        }
    }

    public void closefillQueuePowScreen()
    {
        MusicManager.instance.PlayClip(6);
        buyfillQueuePowScreen.SetActive(false);
    }

    public void closeDestroyObstaclePowScreen()
    {
        MusicManager.instance.PlayClip(6);
        buyDestroyObstaclePowScreen.SetActive(false);
    }
    public void AddDestroyObstaclePow()
    {
        if (CoinsManager.instance.SpendCoins(170))
        {
            DestroyObstaclePow_num++;
            MusicManager.instance.PlayClip(16);
            GameObject remp = Instantiate(added_img , CanvasManager.instance.transform);
            Destroy(remp, 2f);
        }
    }


    public void DestroyObstaclebtn()
    {
        if (DestroyObstaclePow_num > 0)
        {
            Timer.Instance.counting_down = false;
            PowerUseTxt_Object.SetActive(true);
            IsThirdPowerUse = true;
            GameManager.Instance.coolDownIMG.SetActive(true);
            DestroyObstaclePow_num--;
        }
        else
        {
            MusicManager.instance.PlayClip(5);
            buyDestroyObstaclePowScreen.SetActive(true);
;        }
    }



    public GameObject Buttons;
    public void FillQueuebtn()
    {
       if (fillQueuePow_AlreadyinUse== false)
        {
            if (FillQueuePow_num > 0)
            {
                Timer.Instance.counting_down = false;
                FillQueuePow_num--;
                Buttons.SetActive(true);
                spotLight1.SetActive(true);
                spotLight2.SetActive(true);
                StartCoroutine(changingLights());
                fillQueuePow_AlreadyinUse = true;
                GameManager.Instance.coolDownPlane.SetActive(true);
            }
            else
            {
                MusicManager.instance.PlayClip(5);
                buyfillQueuePowScreen.SetActive(true);
                print("Not enough freeze power");
            }
            
        }
        else
        {
            print("Power Already in use");
        }

    }

    [Header("Lights")]

    public Light directionLight;
    public GameObject spotLight1;
    public GameObject spotLight2;
    float duration = .5f;

    IEnumerator changingLights()
    {

        float StartSpotAngle = spotLight1.GetComponent<Light>().spotAngle;
        float finalSpotAngle = 90;

        Color startColor = directionLight.color;

        Color targetColor;
        ColorUtility.TryParseHtmlString("#808080" , out  targetColor);

        float time = 0;

        while (time < .2f)
        {
            //For spot lights
            spotLight1.GetComponent<Light>().spotAngle = Mathf.Lerp(StartSpotAngle, finalSpotAngle, time/ .2f);
            spotLight2.GetComponent<Light>().spotAngle = Mathf.Lerp(StartSpotAngle, finalSpotAngle, time / .2f);

            // Interpolate between start and target color
            directionLight.color = Color.Lerp(startColor, targetColor, time / duration);

            time += Time.deltaTime; // move time forward
            yield return null; // wait for next frame
        }

       
        directionLight.color = targetColor;
        yield return null;

    }

    void closeOrReset_Lights()
    {
        directionLight.color = Color.white;
        spotLight1.SetActive(false);
        spotLight2.SetActive(false);
        spotLight1.GetComponent<Light>().spotAngle = 180;
        spotLight2.GetComponent<Light>().spotAngle = 180;

    }

    public void Button_left()
    {
        MusicManager.instance.PlayClip(16);
        // MusicManager.instance.PlayClip(1);
        StartCoroutine(transfer(0));
        Buttons.SetActive(false);
        Timer.Instance.counting_down = true;

    }
    public void Button_right()
    {
        MusicManager.instance.PlayClip(16);
        //MusicManager.instance.PlayClip(1);
        StartCoroutine(transfer(1));
        Buttons.SetActive(false);
        Timer.Instance.counting_down = true;
    }

    public bool IsPowerUse = false;

    IEnumerator transfer(int index)
    {
        Timer.Instance.counting_down = true;
        closeOrReset_Lights();
       IsPowerUse = true;
        if (GameObject.FindGameObjectWithTag("Pink Queue") != null)
        {
           
            
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Pink Queue").transform.position, GameManager.Instance.spawnPos[index].position) < 0.4f)
            {
                
                GameObject[] block = GameObject.FindGameObjectsWithTag("Pink block");
                for (int i = 0; i < block.Length; i++)
                {
                    
                    block[i].tag = "Untagged";
                   
                    GameManager.Instance.transfer(block[i]);
                    GameManager.Instance.no_of_places_occupied++;
                   
                    
                    yield return new WaitForSeconds(.2f);
                }

            }
        }
        if (GameObject.FindGameObjectWithTag("Red Queue") != null)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Red Queue").transform.position, GameManager.Instance.spawnPos[index].position) < 0.4f)
            {
               
                GameObject[] block = GameObject.FindGameObjectsWithTag("Red block");
                for (int i = 0; i < block.Length; i++)
                {
                    
                    block[i].tag = "Untagged";
                    GameManager.Instance.transfer(block[i]);
                    GameManager.Instance.no_of_places_occupied++;
                   
                    yield return new WaitForSeconds(.2f);
                }


            }
        }
        if (GameObject.FindGameObjectWithTag("Blue Queue") != null)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Blue Queue").transform.position, GameManager.Instance.spawnPos[index].position) < 0.4f)
            {
                GameObject[] block = GameObject.FindGameObjectsWithTag("Blue block");
                for (int i = 0; i < block.Length; i++)
                {
                   
                    block[i].tag = "Untagged";
                    GameManager.Instance.transfer(block[i]);
                    GameManager.Instance.no_of_places_occupied++;

                    yield return new WaitForSeconds(.2f);
                }

            }
        }
        if (GameObject.FindGameObjectWithTag("Green Queue") != null)
        {
            
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Green Queue").transform.position, GameManager.Instance.spawnPos[index].position) < 0.4f)
            {
                
                GameObject[] block = GameObject.FindGameObjectsWithTag("Green block");
               
                for (int i = 0; i < block.Length; i++)
                {
                    
                    block[i].tag = "Untagged";
                    GameManager.Instance.transfer(block[i]);
                    GameManager.Instance.no_of_places_occupied++;

                    yield return new WaitForSeconds(.2f);
                }

            }
        }
        if (GameObject.FindGameObjectWithTag("Yellow Queue") != null)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Yellow Queue").transform.position, GameManager.Instance.spawnPos[index].position) < 0.4f)
            {
                GameObject[] block = GameObject.FindGameObjectsWithTag("Yellow block");
                for (int i = 0; i < block.Length; i++)
                {
                   
                    block[i].tag = "Untagged";
                    GameManager.Instance.transfer(block[i]);
                    GameManager.Instance.no_of_places_occupied++;

                    yield return new WaitForSeconds(.2f);
                }

            }
        }
        if (GameObject.FindGameObjectWithTag("Violet Queue") != null)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Violet Queue").transform.position, GameManager.Instance.spawnPos[index].position) < 0.4f)
            {
                GameObject[] block = GameObject.FindGameObjectsWithTag("Violet block");
                for (int i = 0; i < block.Length; i++)
                {
                   
                    block[i].tag = "Untagged";
                    GameManager.Instance.transfer(block[i]);
                    GameManager.Instance.no_of_places_occupied++;

                    yield return new WaitForSeconds(.2f);
                }

            }
        }
       
        
    }

}
