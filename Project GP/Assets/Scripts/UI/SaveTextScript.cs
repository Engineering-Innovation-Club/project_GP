using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveTextScript : MonoBehaviour
{
    public float timeAppear;
    private bool fading;
    private TextMeshProUGUI text;
    public string message;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timeAppear = 5f;
        text.canvasRenderer.SetAlpha(0);
        text.CrossFadeAlpha(0, 0f, false);
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(message);
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
        timeAppear = 5f;
        fading = true;

        text.canvasRenderer.SetAlpha(0.01f);
        text.CrossFadeAlpha(1, 3f, false);
    }
}
