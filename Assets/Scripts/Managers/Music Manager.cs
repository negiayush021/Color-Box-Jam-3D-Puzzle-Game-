using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip[] clip;
    public AudioSource audioSource_Sound;
    public AudioSource audioSource_music;

    public bool can_play_sound = true;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
        

    }

     bool onetime = true;
    private void Start()
    {



        if (onetime)
        {
            audioSource_music.Play();
            onetime = false;
        }

        audioSource_music.volume = .5f;
    }
    public void PlayClip(int index)
    {
        if (index >= 0 && index < clip.Length)
        {
            if (can_play_sound)
            {
                audioSource_Sound.PlayOneShot(clip[index]);
            }
            
        }
    }
}
