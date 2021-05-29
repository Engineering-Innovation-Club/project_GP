using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public static bool hasDialogue;

    public TextMeshProUGUI speaker;
    public TextMeshProUGUI message;
    public Image portrait;

    private bool doneMessage;

    public GameObject panel;
    private List<string> speakerList;
    private List<string> messageList;
    private List<Image> imageList;

    // Start is called before the first frame update
    void Start()
    {
        hasDialogue = false;
        doneMessage = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (speakerList.Count > 0)
        {
            hasDialogue = true;
        }
    }

    void AddDialogue(string speaker, string message, Image picture)
    {
        speakerList.Add(speaker);
        messageList.Add(message);
        imageList.Add(picture);
    }

    void StartDialogue()
    {
        speaker.text = speakerList[0];
        message.text = messageList[0];
        portrait = imageList[0];
    }

    void NextMessage()
    {
        speakerList.RemoveAt(0);
        messageList.RemoveAt(0);
        imageList.RemoveAt(0);

        StartDialogue();
    }

    private void OnMouseDown()
    {
        if (hasDialogue)
        {
            if (doneMessage)
            {
                NextMessage();
            }
            else
            {
                //FinishMessage();
            }
        }
    }
}
