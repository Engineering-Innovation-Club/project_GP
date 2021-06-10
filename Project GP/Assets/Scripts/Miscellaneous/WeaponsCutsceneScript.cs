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
    public List<Image> imageList;

    // Start is called before the first frame update
    void Start()
    {
        StartCutscene();
        Invoke("Add", (float)clip.duration);
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
            dScript.AddDialogue(speakerList[i], messageList[i], imageList[i]);
        }
    }

    void StartCutscene()
    {
        director.playableAsset = clip;
        director.Play();
    }
}
