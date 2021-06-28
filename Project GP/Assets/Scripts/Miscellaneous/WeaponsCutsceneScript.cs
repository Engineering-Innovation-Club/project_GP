using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class WeaponsCutsceneScript : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset clip;

    public GameObject background;
    public GameObject glass;

    public Sprite destroyedBG;
    public Sprite destroyedGlass;

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

    void SwitchSprites()
    {
        background.GetComponent<SpriteRenderer>().sprite = destroyedBG;
        glass.GetComponent<SpriteRenderer>().sprite = destroyedGlass;
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
