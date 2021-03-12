using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip mainMenu;

    public AudioClip desertPitIntro;

    public AudioClip desertPitLoop;

    public static SoundManager Instance;

    AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }//SINGLETON

    public Magic magic;

    [System.Serializable]
    public class Magic
    {
        public AudioClip explosion;

        public AudioClip heal;

        public AudioClip earthWall;

        public AudioClip earthWallOff;

        public AudioClip sprint;

        public AudioClip electricCharge;

        public AudioClip electricExplosion;

        public AudioClip campfire;

        public AudioClip wind;

        public AudioClip waterFlow;

        //public AudioClip 

    }

    public UI ui;

    [System.Serializable]
    public class UI
    {
       public AudioClip moveTotem;

       public AudioClip enterTotem;

       public AudioClip moveInPlayerSelection;

        public AudioClip AbuttonPlayerSelection;

        public AudioClip BbuttonPlayerSelection;

       public AudioClip start;

       public AudioClip pauseOn;

       public AudioClip pauseOff;
    }

    public PlayerSFX playerSFX; 

    [System.Serializable]
    public class PlayerSFX
    {
       public AudioClip bricHit;
       public AudioClip bricDeath;
       public AudioClip bricEOCharge;
       public AudioClip bricEORelease;

       public AudioClip okkiHit;
       public AudioClip okkiDeath;
       public AudioClip okkiEO;

       public AudioClip stuartoHit;
       public AudioClip stuartoDeath;
       public AudioClip stuartoEOStart;
       public AudioClip stuartoEOEnd;

    }

    public Events evento;

    [System.Serializable]
    public class Events
    {
        public AudioClip suddenDeath;

        public AudioClip suddenDeathMusic;

        public AudioClip endGame;

        public AudioClip cheers;
    }


    public void Reproducir(AudioClip x, float vol = 1f)
    {
        if (audioSource != null)
            audioSource.PlayOneShot(x, vol);
    }

    public void GetAudioSource()
    {
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
    }
}
