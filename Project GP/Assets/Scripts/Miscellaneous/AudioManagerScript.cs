using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    private AudioSource source;

    [SerializeField]
    AudioClip bg, boss;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = bg;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBoss()
    {
        source.clip = boss;
        source.Play();
    }
}
