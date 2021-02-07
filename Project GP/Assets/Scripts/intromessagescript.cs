using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class intromessagescript : MonoBehaviour
{
    //inspector variables. textValue = message to print. fadeTime = how long fade lasts. duration = length of message. Includes fade time. All time is in seconds.
    public string textValue;
    public Text textElement;
    public int fadeTime;
    public int duration;

    //time = timer that counts up. fadeOutTime  = when to begin fading out.
    float time;
    int fadeOutTime;

    // Start is called before the first frame update
    void Start()
    {
        //initiate timer. determine when to begin fading out.
        time = 0;
        fadeOutTime = duration - fadeTime;

        //set message and text alpha to 0 and fade in.
        textElement.CrossFadeAlpha(0, 0.0f, false);
        textElement.text = textValue;
        textElement.CrossFadeAlpha(1, (float)fadeTime, false);
    }

    // Update is called once per frame
    void Update()
    {
        //begin fading out once the timer reaches the fade out time
        if (time >= fadeOutTime)
        {
            //for some reason, when I use CrossFadeAlpha for fading out, it fades super slowly unless I divide the fadeTime smaller? If anyone knows the issue, feel free to fix and lmk.
            textElement.CrossFadeAlpha(0, (fadeTime/2.99f), false);
        }
        //go to main menu when time reaches duration
        if (time >= duration)
        {
            SceneManager.LoadScene("Main Menu");
        }
        //time increases incrementally
        time += Time.deltaTime;
    }
}
