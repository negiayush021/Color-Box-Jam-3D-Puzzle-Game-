using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;


    private void Awake()
    {
        Instance = this;
    }


    [SerializeField] TextMeshProUGUI timer_text;
    [SerializeField] float remaining_time;
    public bool counting_down;


    private void Start()
    {
        counting_down = false;
    }

    bool play_once = true;
    private void Update()
    {
        if (remaining_time > 0)
        {
           if (counting_down)
            {
                remaining_time -= Time.deltaTime;
            }
            else
            {
                //timing stops 
            }
        }
        else if (remaining_time < 0)
        {
            remaining_time = 0;
            CanvasManager.instance.RunOutOfTime_screen.SetActive(true);
            //GameManager.Instance.restart();
        }

        if (remaining_time < 10)
        {
            if (play_once)
            {
                MusicManager.instance.PlayClip(11);
                play_once = false;
            }
           
            timer_text.color = Color.red;
        }
        int minutes = Mathf.FloorToInt(remaining_time / 60);
        int seconds = Mathf.FloorToInt(remaining_time % 60);
        timer_text.text = string.Format("{0:00}:{1:00}" , minutes , seconds);
    }
}
