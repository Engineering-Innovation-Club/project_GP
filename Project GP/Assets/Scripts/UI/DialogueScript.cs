using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public List<Image> imageList;

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
            Debug.Log("Click");
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

    public void AddDialogue(string speaker, string message, Image picture)
    {
        Debug.Log("Add Dialogue");
        speakerList.Add(speaker);
        messageList.Add(message);
        imageList.Add(picture);
    }

    public void StartDialogue()
    {
        Debug.Log("Start Dialogue");
        speaker.text = speakerList[0];
        message.text = messageList[0];
        portrait = imageList[0];

        panel.SetActive(true);
    }

    public void EndDialogue()
    {
        Debug.Log("End Dialogue");
        speakerList.RemoveAt(0);
        messageList.RemoveAt(0);
        imageList.RemoveAt(0);

        panel.SetActive(false);
    }

    void NextMessage()
    {
        Debug.Log("Next Dialogue");
        speakerList.RemoveAt(0);
        messageList.RemoveAt(0);
        imageList.RemoveAt(0);

        StartDialogue();
    }
}
