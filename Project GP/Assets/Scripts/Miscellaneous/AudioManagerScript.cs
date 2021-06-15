using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public AudioSource source;

    [SerializeField]
    AudioClip bg, boss, labCutscene;

    public bool fadeIn;
    public bool fadeOut;
    public float fadeFactor;
    public float maxVolume;

    // Start is called before the first frame update
    void Start()
    {
        fadeIn = false;

        source = GetComponent<AudioSource>();
        source.clip = bg;
        source.volume = maxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            if (source.volume < maxVolume)
            {
                source.volume += fadeFactor;
            }
            else if (source.volume >= maxVolume)
            {
                source.volume = maxVolume;
                fadeIn = false;
            }
        }

        if (fadeOut)
        {
            if (source.volume > 0)
            {
                source.volume += fadeFactor;
            }
            else if (source.volume <= 0)
            {
                source.volume = 0;
                fadeOut = false;
            }
        }
    }

    public void PlayBG()
    {
        source.clip = bg;

        source.Play();
        source.loop = true;

        source.volume = 0;
        fadeIn = true;

        
    }

    public void PlayBoss()
    {
        source.clip = boss;
        source.Play();
        source.loop = true;
    }

    public void PlayLC()
    {
        source.clip = labCutscene;
        source.Play();
        source.loop = false;
    }
}
