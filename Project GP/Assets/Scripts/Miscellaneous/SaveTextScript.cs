using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTextScript : MonoBehaviour
{
    private Text text;
    public float timeAppear;
    private bool fading;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        timeAppear = 5f;
        text.canvasRenderer.SetAlpha(0);
        text.CrossFadeAlpha(0, 0f, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            timeAppear -= Time.deltaTime;
        }

        if (timeAppear <= 0)
        {
            text.CrossFadeAlpha(0, 0.5f, false);
            fading = false;
        }
        
    }

    public void FadeAnimation()
    {
        Debug.Log("fading");
        timeAppear = 5f;
        fading = true;

        text.canvasRenderer.SetAlpha(0.01f);
        text.CrossFadeAlpha(1, 3f, false);
    }
}
