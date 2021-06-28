using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class DialogueScript : MonoBehaviour
{
    public bool hasDialogue;

    public TextMeshProUGUI speaker;
    public TextMeshProUGUI message;
    public Image portrait;

    private bool lastMessage;

    public GameObject panel;
    public List<string> speakerList;
    public List<string> messageList;
    public List<Sprite> imageList;
    public List<PlayableAsset> animList;

    public PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        hasDialogue = false;
        lastMessage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (speakerList.Count > 0)
        {
            hasDialogue = true;
        }
        else
        {
            hasDialogue = false;
        }

        if (speakerList.Count == 1)
        {
            lastMessage = true;
        }
        else
        {
            lastMessage = false;
        }

        if (hasDialogue && Input.GetMouseButtonDown(0))
        {
            if (hasDialogue)
            {
                if (!lastMessage)
                {
                    NextMessage();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    public void AddDialogue(string speaker, string message, Sprite picture, PlayableAsset clip)
    {
        speakerList.Add(speaker);
        messageList.Add(message);
        imageList.Add(picture);
        animList.Add(clip);
    }

    public void StartDialogue()
    {
        speaker.text = speakerList[0];
        message.text = messageList[0];

        if (imageList[0] != null)
        {
            portrait.sprite = imageList[0];
        }
        

        if (animList[0] != null)
        {
            director.playableAsset = animList[0];
            director.Play();
        }

        panel.SetActive(true);
    }

    public void EndDialogue()
    {
        speakerList.RemoveAt(0);
        messageList.RemoveAt(0);
        imageList.RemoveAt(0);
        animList.RemoveAt(0);

        panel.SetActive(false);
    }

    void NextMessage()
    {
        speakerList.RemoveAt(0);
        messageList.RemoveAt(0);
        imageList.RemoveAt(0);
        animList.RemoveAt(0);

        StartDialogue();
    }
}
