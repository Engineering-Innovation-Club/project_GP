using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponsCutsceneScript : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset clip;

    public GameObject background;
    public GameObject glass;
    public GameObject labBG;

    public Sprite destroyedBG;
    public Sprite destroyedGlass;
    public Sprite destroyedLab;
    public Light2D bigLight;
    public Light2D smallLight;
    public Light2D smolLight;

    public DialogueScript dScript;

    public List<string> speakerList;
    public List<string> messageList;
    public List<Sprite> imageList;
    public List<PlayableAsset> animList;

    public AudioManagerScript audioScript;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        StartCutscene();
        Invoke("PlayDialogue", (float)clip.duration);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= (float)clip.duration + 1f && !audioScript.source.isPlaying) {
            if (!dScript.hasDialogue)
            {
                audioScript.PlayBG();
                timer = 0f;
            }
        }
    }

    void DestroyLab()
    {
        background.GetComponent<SpriteRenderer>().sprite = destroyedBG;
        glass.GetComponent<SpriteRenderer>().sprite = destroyedGlass;
        labBG.GetComponent<SpriteRenderer>().sprite = destroyedLab;
    }

    void SwitchNight()
    {
        bigLight.color = new Color(173/255, 117/255, 198/255);
        smallLight.color = new Color(132 / 255, 82 / 255, 178 / 255);
        smolLight.color = new Color(132 / 255, 82 / 255, 178 / 255);
    }

    void Add()
    {
        for (int i = 0; i < speakerList.Count; i++)
        {
            dScript.AddDialogue(speakerList[i], messageList[i], imageList[i], animList[i]);
        }
    }

    void PlayDialogue()
    {
        Add();
        dScript.StartDialogue();
    }

    void StartCutscene()
    {
        director.playableAsset = clip;
        director.Play();
    }
}
