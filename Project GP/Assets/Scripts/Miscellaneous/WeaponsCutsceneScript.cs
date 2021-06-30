using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponsCutsceneScript : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset part1;
    public PlayableAsset part2;
    public PlayableAsset part3;

    public GameObject beforeLab;
    public GameObject afterLights;
    public GameObject healthbar;
    public GameObject water;

    public DialogueScript dScript;

    public int part;

    public List<string> speakerList;
    public List<string> messageList;
    public List<Sprite> imageList;
    public List<PlayableAsset> animList;

    public List<string> speakerList2;
    public List<string> messageList2;
    public List<Sprite> imageList2;
    public List<PlayableAsset> animList2;

    public List<string> speakerList3;
    public List<string> messageList3;
    public List<Sprite> imageList3;
    public List<PlayableAsset> animList3;

    public AudioManagerScript audioScript;

    private float timer;
    private float fadeTimer;
    private bool fade;
    public static bool inCutscene;
    public GameObject player;
    public GameObject robot;
    public GameObject kanaan;

    // Start is called before the first frame update
    void Start()
    {
        fade = false;
        StartCutscene();
        Invoke("Part1Dialogue", (float)part1.duration);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        fadeTimer += Time.deltaTime;
        if (timer >= (float)part1.duration + 1f && part == 1) {
            if (!dScript.hasDialogue)
            {
                part += 1;
                timer = 0f;
                fadeTimer = 0f;
                fade = true;
                FadeToBlackScript.fading = true;
            }
        }

        if (fadeTimer >= 5f && fade && part == 2)
        {
            fade = false;
            FadeToBlackScript.fading = false;
            PlayPart2();
            Invoke("Part2Dialogue", (float)part2.duration);
            timer = 0f;
        }

        if (timer >= (float)part2.duration + 1f && part == 2 && !fade)
        {
            if (!dScript.hasDialogue)
            {
                part += 1;
                timer = 0f;
                fadeTimer = 0f;
                fade = true;
                FadeToBlackScript.fading = true;
                
            }
        }

        if (fadeTimer >= 5f && fade && part == 3)
        {
            fade = false;
            FadeToBlackScript.fading = false;
            PlayPart3();
            Invoke("Part3Dialogue", (float)part3.duration);
            timer = 0f;
        }

        if (timer >= (float)part3.duration + 1f && part == 3 && !fade)
        {
            if (!dScript.hasDialogue && !audioScript.source.isPlaying)
            {
                fade = true;
                fadeTimer = 0f;
                FadeToBlackScript.fading = true;
                part += 1;
    
            }
        }

        if (fadeTimer >= 5f && fade && part == 4)
        {
            fade = false;
            FadeToBlackScript.fading = false;
            audioScript.PlayIntro();
            healthbar.SetActive(true);
            inCutscene = false;
            water.SetActive(true);
            beforeLab.SetActive(false);
            afterLights.SetActive(true);
            player.transform.localScale = new Vector2(-player.transform.localScale.x, player.transform.localScale.y);
            kanaan.SetActive(false);
        }
    }

    void DestroyLab()
    {
        beforeLab.SetActive(false);
        afterLights.SetActive(true);
    }

    void Add(List<string> sList, List<string> mList, List<Sprite> iList, List<PlayableAsset> aList)
    {
        for (int i = 0; i < sList.Count; i++)
        {
            dScript.AddDialogue(sList[i], mList[i], iList[i], aList[i]);
        }
    }

    void Part1Dialogue()
    {
        Add(speakerList, messageList, imageList, animList);
        dScript.StartDialogue();
    }

    void Part2Dialogue()
    {
        Add(speakerList2, messageList2, imageList2, animList2);
        dScript.StartDialogue();
    }

    void Part3Dialogue()
    {
        Add(speakerList3, messageList3, imageList3, animList3);
        dScript.StartDialogue();
    }

    void StartCutscene()
    {
        director.playableAsset = part1;
        director.Play();
        inCutscene = true;
    }

    void PlayPart2()
    {
        director.playableAsset = part2;
        director.Play();
        robot.SetActive(true);
    }
    
    void PlayPart3()
    {
        director.playableAsset = part3;
        director.Play();
    }
}
